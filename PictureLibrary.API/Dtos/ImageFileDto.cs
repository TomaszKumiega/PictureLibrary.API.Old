namespace PictureLibrary.API.Dtos
{
    public class ImageFileDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Extension { get; set; }
        public required long Size { get; set; }
    }
}