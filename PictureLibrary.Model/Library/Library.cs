namespace PictureLibrary.Model
{
    public class Library
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
