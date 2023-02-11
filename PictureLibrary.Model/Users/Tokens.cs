namespace PictureLibrary.Model
{
    public class Tokens
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime ExpiryDate { get; set; }
    }
}
