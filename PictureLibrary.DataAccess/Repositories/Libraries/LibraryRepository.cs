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

        public async Task<Library?> FindByIdAsync(Guid id)
        {
            var parameters = new { Id = id };
            string sql = @"
SELECT * FROM Libraries
WHERE Id = @Id";

            var libraries = await _databaseAccess.LoadDataAsync(sql, parameters);
            return libraries.FirstOrDefault();
        }

        public async Task AddLibrary(Library library)
        {
            string sql = @"
INSERT INTO Libraries (Id, Name, Description)
VALUES (@Id, Name, Description)";

            await _databaseAccess.SaveDataAsync(sql, library);
        }
    }
}
