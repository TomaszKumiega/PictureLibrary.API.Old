using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient
{
    public class UserClient : ClientBase, IUserClient
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

        public async Task<IEnumerable<User>> FindUserByPartOfUsernameAsync(AuthorizationData authorizationData, string partOfUsername)
        {
            var response = await SendRequestAndDeserializeResponseAsync<FindUsersResponse>(HttpMethod.Get, $"users/find/{partOfUsername}", authorizationData: authorizationData);
            return response?.Users ?? Enumerable.Empty<User>();
        }

        public async Task UpdateUserAsync(AuthorizationData authorizationData, Guid userId, UpdateUserRequest request)
        {
            await SendRequest(HttpMethod.Patch, $"users/update/{userId}", request, authorizationData);
        }

        public async Task<User?> GetUserAsync(AuthorizationData authorizationData, Guid userId)
        {
            return await SendRequestAndDeserializeResponseAsync<User>(HttpMethod.Get, $"users/{userId}", authorizationData: authorizationData);
        }
    }
}
