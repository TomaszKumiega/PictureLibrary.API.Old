using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface IImageFileRepository
    {
        Task<IEnumerable<ImageFile>> GetAll(Guid libraryId);
    }
}