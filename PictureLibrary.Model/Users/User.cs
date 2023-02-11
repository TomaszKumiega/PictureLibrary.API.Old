namespace PictureLibrary.Model
{
    public class User
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required string EmailAddress { get; set; }
        public required string Role { get; set; }
    }
}
