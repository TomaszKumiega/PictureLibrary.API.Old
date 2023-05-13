using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;

namespace PictureLibrary.APIClient.ImageFiles
{
    public interface IImageFileClient
    {
        Task<Guid?> AddImageFileAsync(AuthorizationData authorizationData, AddImageFileRequest addImageFileRequest, Stream imageFileContent);
        Task DeleteImageFileAsync(AuthorizationData authorizationData, Guid imageFileId);
        Task<IEnumerable<ImageFile>> GetAllImageFilesAsync(AuthorizationData authorizationData, Guid libraryId);
        Task UpdateImageFileAsync(AuthorizationData authorizationData, Guid imageFileId, UpdateImageFileRequest updateImageFileRequest);
    }
}