using System.Net;

namespace PictureLibrary.API
{
    public class ErrorResponse
    {
        public required HttpStatusCode StatusCode { get; set; }
        public required string Title { get; set; }
        public List<ErrorDetails>? Details { get; set; }
    }
}
