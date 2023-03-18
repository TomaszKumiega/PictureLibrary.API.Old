using PictureLibrary.Model;

namespace PictureLibrary.Api.Dtos
{
    public class NewLibraryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required List<User> Owners { get; set; }
    }
}
