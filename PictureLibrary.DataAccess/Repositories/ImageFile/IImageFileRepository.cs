using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface IImageFileRepository
    {
        Task<IEnumerable<ImageFile>> GetAll(Guid libraryId);
        Task<Guid> AddImageFile(ImageFile imageFile);
        Task<ImageFile?> FindImageFileById(Guid id);
        Task DeleteImageFile(Guid imageFileId);
    }
}