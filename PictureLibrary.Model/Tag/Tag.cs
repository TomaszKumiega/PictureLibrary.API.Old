namespace PictureLibrary.Model
{
    public class Tag
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string ColorHex { get; set; }
    }
}
