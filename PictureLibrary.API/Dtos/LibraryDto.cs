using PictureLibrary.Model;

namespace PictureLibrary.Api.Dtos
{
    public class LibraryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required List<User> Owners { get; set; }
    }
}
