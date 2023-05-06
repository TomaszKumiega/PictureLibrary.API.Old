using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient
{
    public interface IUserClient
    {
        Task DeleteUserAsync(AuthorizationData authorizationData, Guid userId);
        Task<IEnumerable<User>> FindUserByPartOfUsernameAsync(AuthorizationData authorizationData, string partOfUsername);
        Task<User?> GetUserAsync(AuthorizationData authorizationData, Guid userId);
        Task<AuthorizationData?> Login(LoginRequest request);
        Task<UserRegisterResponse?> RegisterUserAsync(UserRegisterRequest request);
        Task UpdateUserAsync(AuthorizationData authorizationData, Guid userId, UpdateUserRequest request);
    }
}