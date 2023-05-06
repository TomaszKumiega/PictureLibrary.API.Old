using PictureLibrary.Model;

namespace PictureLibrary.API.Dtos
{
    public class LibraryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required List<UserDto> Owners { get; set; }
    }
}
