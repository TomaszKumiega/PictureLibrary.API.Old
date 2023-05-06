namespace PictureLibrary.APIClient.Model.Requests
{
    public class LoginRequest : IRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
