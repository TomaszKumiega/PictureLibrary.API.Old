using PictureLibrary.Model.Users;

namespace PictureLibrary.API.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string EmailAddress { get; set; }
        public required UserRole Role { get; set; }
    }
}
