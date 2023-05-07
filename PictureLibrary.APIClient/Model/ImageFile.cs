namespace PictureLibrary.APIClient.Model
{
    public class ImageFile
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Extension { get; set; }
        public required long Size { get; set; }
    }
}
