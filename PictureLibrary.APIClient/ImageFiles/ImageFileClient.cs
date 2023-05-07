using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient.ImageFiles
{
    public class ImageFileClient : ClientBase
    {
        public async Task<IEnumerable<ImageFile>> GetAllImageFilesAsync(AuthorizationData authorizationData, Guid libraryId)
        {
            var response = await SendRequestAndDeserializeResponseAsync<GetAllImageFilesResponse>(HttpMethod.Get, $"imageFile/all/{libraryId}", null, authorizationData);
            return response?.ImageFiles ?? Enumerable.Empty<ImageFile>();
        }
    }
}
