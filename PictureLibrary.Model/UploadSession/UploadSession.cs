namespace PictureLibrary.Model.UploadSession
{
    public class UploadSession
    {
        public Guid Id { get; set; }
        public Guid? UpdatedImageFileId { get; set; }
        public required UploadSessionType Type { get; set; }
        public required string ContentRange { get; set; }
        public required string FilePath { get; set; }
        public required Guid UserId { get; set; }
        public required List<Library> Libraries { get; set; }
    }
}
