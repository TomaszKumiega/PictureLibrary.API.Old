namespace PictureLibrary.Model
{
    public class Library
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
