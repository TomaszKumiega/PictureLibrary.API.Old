namespace PictureLibrary.Model
{
    public class Tag
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string ColorHex { get; set; }

        public List<Library>? Libraries { get; set; }
    }
}
