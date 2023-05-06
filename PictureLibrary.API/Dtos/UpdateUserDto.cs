using PictureLibrary.Model.Users;

namespace PictureLibrary.API.Dtos
{
    public class UpdateUserDto
    {
        public required string Username { get; set; }
        public required string EmailAddress { get; set; }
    }
}
