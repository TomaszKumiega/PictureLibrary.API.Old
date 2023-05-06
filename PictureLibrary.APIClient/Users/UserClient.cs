using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient
{
    public class UserClient : ClientBase
    {   
        public async Task<AuthorizationData?> Login(LoginRequest request)
        {
            return await SendRequestAndDeserializeResponseAsync<AuthorizationData>(HttpMethod.Post, "authorization/login", request);
        }

        public async Task<UserRegisterResponse?> RegisterUserAsync(UserRegisterRequest request)
        {
            return await SendRequestAndDeserializeResponseAsync<UserRegisterResponse>(HttpMethod.Post, "users/register", request);
        }

        public async Task DeleteUserAsync(AuthorizationData authorizationData, Guid userId)
        {
            await SendRequest(HttpMethod.Delete, $"users/delete/{userId}", null, authorizationData);
        }

        public async Task<IEnumerable<User>> FindUserByPartOfUsername(AuthorizationData authorizationData, string partOfUsername)
        {
            var response = await SendRequestAndDeserializeResponseAsync<FindUsersResponse>(HttpMethod.Get, $"users/find/{partOfUsername}", authorizationData: authorizationData);
            return response?.Users ?? Enumerable.Empty<User>();
        }
    }
}
