namespace PictureLibrary.APIClient.Model.Requests
{
    public class AddTagRequest : IRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ColorHex { get; set; }
        public List<Guid>? Libraries { get; set; }
    }
}
