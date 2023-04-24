using System.Text.Json.Serialization;

namespace PictureLibrary.Model
{
    public class Tokens
    {
        [JsonIgnore]
        public required Guid Id { get; set; }
        [JsonIgnore]
        public required Guid UserId { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
