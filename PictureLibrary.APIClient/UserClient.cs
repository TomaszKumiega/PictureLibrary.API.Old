using PictureLibrary.APIClient.Model.Requests;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient
{
    public class UserClient : ClientBase
    {   
        public async Task<UserRegisterResponse?> RegisterUserAsync(UserRegisterRequest request)
        {
            return await SendRequestAndDeserializeResponseAsync<UserRegisterResponse>("users/register", request);
        }
    }
}
