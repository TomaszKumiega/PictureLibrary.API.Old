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
    }
}
