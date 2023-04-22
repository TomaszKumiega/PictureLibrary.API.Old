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

        public async Task<Guid> AddLibrary(Library library)
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

        public async Task DeleteLibrary(Library library)
        {
            await DeleteRelatedEntities(library.Id);
            
            string sql = @"
DELETE FROM Libraries
WHERE Library.Id = @LibraryId";

            await _databaseAccess.SaveDataAsync(sql, new { LibraryId = library.Id });
        }

        #region Private methods
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

        private async Task DeleteRelatedEntities(Guid libraryId)
        {
            await DeleteRelatedTags(libraryId);
            await DeleteRelatedImageFiles(libraryId);
            await DeleteOwners(libraryId);
        }

        private async Task DeleteRelatedTags(Guid libraryId)
        {
            string deleteLibraryTagSql = @"
DELETE FROM LibraryTags
WHERE LibraryId = @LibraryId";

            await _databaseAccess.SaveDataAsync(deleteLibraryTagSql, new { LibraryId = libraryId });

            string getOrphanedTagsSql = @"
SELECT _tag.Id FROM Tags _tag
LEFT JOIN LibraryTags _libraryTag
ON _tag.Id = _libraryTag.Id
WHERE _libraryTag.Id IS NULL";

            var orphanedTagsIds = await _databaseAccess.LoadDataAsync<Guid>(getOrphanedTagsSql, null!);

            string deleteTagSql = @"
DELETE FROM Tags 
WHERE Id = @TagId";

            foreach (var tagId in orphanedTagsIds)
            {
                await _databaseAccess.SaveDataAsync(deleteTagSql, new { TagId = tagId });   
            }
        }

        private async Task DeleteRelatedImageFiles(Guid libraryId)
        {
            string deleteLibraryImageFiles = @"
DELETE FROM LibraryImageFiles
WHERE LibraryId = @LibraryId";

            await _databaseAccess.SaveDataAsync(deleteLibraryImageFiles, new { LibraryId = libraryId });

            string getOrphanedImageFileIds = @"
SELECT _imageFile.Id FROM ImageFiles _imageFile
LEFT JOIN LibraryImageFiles _libraryImageFile
ON _imageFile.Id = _libraryImageFile.Id
WHERE _libraryImageFile.Id IS NULL";  

            var orphanedImageFiles = await _databaseAccess.LoadDataAsync<Guid>(getOrphanedImageFileIds, null!);

            string deleteImageFile = @"
DELETE FROM ImageFiles
WHERE Id = @ImageFileId";

            foreach (var imageId in orphanedImageFiles)
            {
                await _databaseAccess.SaveDataAsync(deleteImageFile, new { ImageId = imageId });
            }
        }

        private async Task DeleteOwners(Guid libraryId)
        {
            string deleteUserLibraries = @"
DELETE FROM UserLibraries
WHERE LibraryId = @LibraryId";

            await _databaseAccess.SaveDataAsync(deleteUserLibraries, new { LibraryId = libraryId });
        }
        #endregion
    }
}
