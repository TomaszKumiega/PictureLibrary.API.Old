using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient
{
    public class UserClient : ClientBase
    {   
        public async Task<UserRegisterResponse?> RegisterUserAsync(UserRegisterRequest request)
        {
            return await SendRequestAndDeserializeResponseAsync<UserRegisterResponse>(HttpMethod.Post, "users/register", request);
        }

        public async Task DeleteUserAsync(AuthorizationData authorizationData, Guid userId)
        {
            await SendRequest(HttpMethod.Delete, $"users/delete/{userId.ToString()}", null, authorizationData);
        }
    }
}
