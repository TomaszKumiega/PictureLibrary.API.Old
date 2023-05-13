namespace PictureLibrary.APIClient.Model.Requests
{
    public class AddImageFileRequest : IRequest
    {
        public required string FileName { get; set; }
        public required List<Guid> Libraries { get; set; }
    }
}
