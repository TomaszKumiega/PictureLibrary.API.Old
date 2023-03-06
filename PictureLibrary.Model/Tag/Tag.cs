namespace PictureLibrary.Model
{
    public class Tag
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string ColorHex { get; set; }
        public required IEnumerable<Library> Libraries { get; set; }
    }
}
