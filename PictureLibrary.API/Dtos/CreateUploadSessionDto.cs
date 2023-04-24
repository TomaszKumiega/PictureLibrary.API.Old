namespace PictureLibrary.API.Dtos
{
    public class CreateUploadSessionDto
    {
        public required string FileName { get; set; }
        public required List<Guid> Libraries { get; set; }
    }
}
