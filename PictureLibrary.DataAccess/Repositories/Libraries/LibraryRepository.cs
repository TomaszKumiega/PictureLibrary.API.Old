using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model;
using System.Linq;

namespace PictureLibrary.DataAccess.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly IDatabaseAccess<Library> _databaseAccess;

        public LibraryRepository(IDatabaseAccess<Library> databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<IEnumerable<Library>> GetAll(Guid userId)
        {
            var parameters = new { UserId = userId };
            string sql = @"
SELECT _library.*, _user.* FROM Libraries _library
LEFT JOIN UserLibraries _userLibrary
ON _library.Id = _userLibrary.LibraryId
LEFT JOIN Users _user
ON _user.Id = _userLibrary.UserId
WHERE _userLibrary.UserId = @UserId";
        
            IEnumerable<(Library Library, User User)> result = await _databaseAccess.LoadDataAsync<Library, User>(sql, (library, user) => (library, user), parameters);

            return result.GroupBy(x => x.Library.Id)
                .Select(g =>
                {
                    var library = g.First().Library;
                    library.Owners = g.Select(x => x.User)
                        .Where(x => x != null)
                        .ToList();

                    return library;
                });
        }

        public async Task<Library?> FindByIdAsync(Guid id)
        {
            var parameters = new { Id = id };
            string sql = @"
SELECT * FROM Libraries _library
LEFT JOIN UserLibraries _userLibrary
ON _library.Id = _userLibrary.LibraryId
LEFT JOIN Users _user
ON _user.Id = _userLibrary.UserId
WHERE _library.Id = @Id";

            IEnumerable<(Library Library, User User)> result = await _databaseAccess.LoadDataAsync<Library, User>(sql, (library, user) => (library, user), parameters);
            
            return result.GroupBy(x => x.Library.Id)
                .Select(g =>
                {
                    var library = g.First().Library;
                    library.Owners = g.Select(x => x.User)
                    .Where(x => x != null)
                    .ToList();

                    return library;
                })
                .FirstOrDefault();
        }

        public async Task<Guid> AddLibrary(Library library, Guid userId)
        {
            library.Id = Guid.NewGuid();

            string addUserSql = @"
INSERT INTO Libraries (Id, Name, Description)
VALUES (@Id, Name, Description)";

            await _databaseAccess.SaveDataAsync(addUserSql, library);

            await UpdateOwners(library);

            return library.Id;
        }

        public async Task UpdateLibrary(Library library)
        {
            string sql = @"
UPDATE Libraries
SET 
Name = @Name,
Description = @Description
WHERE Id = @Id";

            await _databaseAccess.SaveDataAsync(sql, library);

            await UpdateOwners(library);
        }

        private async Task UpdateOwners(Library library, bool isNewLibrary = false)
        {
            IEnumerable<Guid> newOwnerIds = library.Owners?.Select(x => x.Id) ?? Enumerable.Empty<Guid>();

            if (!isNewLibrary)
            {
                string sql = @"
SELECT UserId FROM UserLibraries
WHERE LibraryId = @LibraryId";

                var userIds = await _databaseAccess.LoadDataAsync<Guid>(sql, new { LibraryId = library.Id });
                var owners = library.Owners?.Select(x => x.Id) ?? Enumerable.Empty<Guid>();
                newOwnerIds = owners.Except(userIds);
                var removedOwners = userIds.Except(owners);

                foreach (var removedOwner in removedOwners)
                {
                    await RemoveOwner(library.Id, removedOwner);
                }
            }


            foreach (var newOwnerId in newOwnerIds)
            {
                await AddOwner(library.Id, newOwnerId);
            }
        }

        private async Task RemoveOwner(Guid libraryId, Guid ownerId)
        {
            var parameters = new
            {
                LibraryId = libraryId,
                OwnerId = ownerId,
            };

            string sql = @"
DELETE FROM UserLibraries
WHERE LibraryId = @LibraryId
AND UserId = @OwnerId";

            await _databaseAccess.SaveDataAsync(sql, parameters);
        }

        private async Task AddOwner(Guid libraryId, Guid ownerId)
        {
            var parameters = new
            {
                Id = Guid.NewGuid(),
                LibraryId = libraryId,
                OwnerId = ownerId,
            };

            string sql = @"
INSERT INTO UserLibraries (Id, LibraryId, UserId)
VALUES (@Id, @LibraryId, @OwnerId)";

            await _databaseAccess.SaveDataAsync(sql, parameters);
        }
    }
}
