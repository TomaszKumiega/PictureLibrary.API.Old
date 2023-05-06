using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient.Tags
{
    public class TagClient : ClientBase
    {
        public async Task<Guid?> AddTagAsync(AuthorizationData authorizationData, AddTagRequest addTagRequest)
        {
            var response = await SendRequestAndDeserializeResponseAsync<AddTagResponse>(HttpMethod.Post, $"tags", addTagRequest, authorizationData);
            return response?.TagId;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync(AuthorizationData authorizationData, Guid libraryId)
        {
            var response = await SendRequestAndDeserializeResponseAsync<GetAllTagsResponse>(HttpMethod.Get, $"tags/{libraryId}", null, authorizationData);
            return response?.Tags ?? Enumerable.Empty<Tag>();
        }
    }
}
