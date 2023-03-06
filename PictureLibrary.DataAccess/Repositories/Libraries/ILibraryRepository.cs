using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface ILibraryRepository
    {
        Task<Guid> AddLibrary(Library library, Guid userId);
        Task<Library?> FindByIdAsync(Guid id);
        Task<IEnumerable<Library>> GetAll(Guid userId);
    }
}