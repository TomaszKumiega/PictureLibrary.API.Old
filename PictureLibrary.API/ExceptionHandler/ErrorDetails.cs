using System.Net;

namespace PictureLibrary.API.ExceptionHandler
{
    public class ErrorDetails
    {
        public required HttpStatusCode StatusCode { get; set; }
        public required string Title { get; set; }
        public string? Details { get; set; }
    }
}
