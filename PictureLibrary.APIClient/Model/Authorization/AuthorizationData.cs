namespace PictureLibrary.APIClient.Model.Authorization
{
    public class AuthorizationData
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime ExpiryDate { get; set; }
    }
}
