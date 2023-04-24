using PictureLibrary.Model.UploadSession;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface IUploadSessionRepository
    {
        Task<Guid> AddUploadSession(UploadSession uploadSession);
        Task DeleteUploadSession(Guid id);
        Task<UploadSession?> FindUploadSessionById(Guid id);
        Task UpdateUploadSession(UploadSession uploadSession);
    }
}