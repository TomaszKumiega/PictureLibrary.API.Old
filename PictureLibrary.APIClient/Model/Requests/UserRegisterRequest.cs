namespace PictureLibrary.APIClient.Model.Requests
{
    public class UserRegisterRequest : IRequest
    {
        public required string Username { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
