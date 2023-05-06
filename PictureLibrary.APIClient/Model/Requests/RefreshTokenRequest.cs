namespace PictureLibrary.APIClient.Model.Requests
{
    public class RefreshTokenRequest : IRequest
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
