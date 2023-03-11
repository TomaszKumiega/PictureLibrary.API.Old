using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model;

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
SELECT * FROM Libraries _library
LEFT JOIN UserLibraries _userLibrary
ON _library.Id = _userLibrary.LibraryId
WHERE _userLibrary.UserId = @UserId";
        
            return await _databaseAccess.LoadDataAsync(sql, parameters);
        }

        public async Task<Library?> FindByIdAsync(Guid id)
        {
            var parameters = new { Id = id };
            string sql = @"
SELECT * FROM Libraries
WHERE Id = @Id";

            var libraries = await _databaseAccess.LoadDataAsync(sql, parameters);
            return libraries.FirstOrDefault();
        }

        public async Task<Guid> AddLibrary(Library library, Guid userId)
        {
            library.Id = Guid.NewGuid();

            string addUserSql = @"
INSERT INTO Libraries (Id, Name, Description)
VALUES (@Id, Name, Description)";

            await _databaseAccess.SaveDataAsync(addUserSql, library);

            string addRelationshipSql = @"
INSERT INTO UserLibraries (LibraryId, UserId)
VALUES (@LibraryId, @UserId)";

            await _databaseAccess.SaveDataAsync(addRelationshipSql, new { LibraryId = library.Id, UserId = userId });

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
        }
    }
}
