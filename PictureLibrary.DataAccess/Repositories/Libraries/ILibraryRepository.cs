using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface ILibraryRepository
    {
        Task AddLibrary(Library library);
        Task<Library?> FindByIdAsync(Guid id);
        Task<IEnumerable<Library>> GetAll(Guid userId);
    }
}