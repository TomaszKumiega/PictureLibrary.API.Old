namespace PictureLibrary.APIClient.Model.Requests
{
    public class UpdateUserRequest : IRequest
    {
        public required string Username { get; set; }
        public required string EmailAddress { get; set; }
    }
}
