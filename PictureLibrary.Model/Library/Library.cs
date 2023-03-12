namespace PictureLibrary.Model
{
    public class Library
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<User>? Owners { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<ImageFile>? ImageFiles { get; set; }
    }
}
