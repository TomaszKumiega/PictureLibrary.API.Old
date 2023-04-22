namespace PictureLibrary.Model.UploadSession
{
    public class UploadSession
    {
        public Guid Id { get; set; }
        public required string ContentRange { get; set; }
        public required string FilePath { get; set; }
    }
}
