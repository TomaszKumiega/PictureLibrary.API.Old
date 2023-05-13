using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;
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

        public async Task DeleteImageFileAsync(AuthorizationData authorizationData, Guid imageFileId)
        {
            await SendRequestAsync(HttpMethod.Delete, $"imageFile/{imageFileId}", null, authorizationData);
        }

        public async Task UpdateImageFileAsync(AuthorizationData authorizationData, Guid imageFileId, UpdateImageFileRequest updateImageFileRequest)
        {
            await SendRequestAsync(HttpMethod.Patch, $"imageFile/{imageFileId}", updateImageFileRequest, authorizationData);
        }

        public async Task<Guid?> AddImageFileAsync(AuthorizationData authorizationData, AddImageFileRequest addImageFileRequest, Stream imageFileContent)
        {
            var response = await SendRequestAndDeserializeResponseAsync<CreateUploadSessionResponse>(HttpMethod.Post, $"imageFile/createUploadSession", addImageFileRequest, authorizationData);

            if (response?.UploadUrl == null)
                return null;

            int maxRequestLength = 500000;
            byte[] buffer = new byte[maxRequestLength];
            int rangeFrom = 0;
            int bytesRead;

            while ((bytesRead = await imageFileContent.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                using var ms = new MemoryStream(buffer, 0, bytesRead);
                string range = $"bytes {rangeFrom}-{rangeFrom + bytesRead - 1}/{imageFileContent.Length}";

                var uploadFileResponse = await SendRequestWithStreamContentAndDeserializeResponseAsync<UploadFileResponse>(HttpMethod.Post, response.UploadUrl, ms, range, authorizationData);

                if (uploadFileResponse != null)
                    return uploadFileResponse.ImageFile?.Id;
            }

            return null;
        }
    }
}
