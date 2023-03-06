namespace PictureLibrary.Model
{
    public class ImageFile
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Extension { get; set; }
        public required long Size { get; set; }
        public required IEnumerable<Library> Libraries { get; set; }
        public required IEnumerable<User> Owners { get; set; }
        public IEnumerable<Tag>? Tags { get; set; }
    }
}
