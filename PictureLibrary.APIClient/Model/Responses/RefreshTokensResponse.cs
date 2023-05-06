namespace PictureLibrary.APIClient.Model.Responses
{
    public class RefreshTokensResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
