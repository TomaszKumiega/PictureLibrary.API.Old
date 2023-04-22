using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface ILibraryRepository
    {
        Task<Guid> AddLibrary(Library library);
        Task<Library?> FindByIdAsync(Guid id);
        Task<IEnumerable<Library>> GetAll(Guid userId);
        Task UpdateLibrary(Library library);
        Task DeleteLibrary(Library library);
    }
}