namespace PictureLibrary.Model
{
    public class Library
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Tag>? Tags { get; set; }
        public IEnumerable<ImageFile>? Images { get; set; }
        public IEnumerable<User>? Owners { get; set; }
    }
}
