namespace PictureLibrary.APIClient.Model.Requests
{
    public class UpdateImageFileRequest : IRequest
    {
        public required string Name { get; set; }
    }
}
