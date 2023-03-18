using Microsoft.AspNetCore.Diagnostics;
using PictureLibrary.DataAccess.Exceptions;
using System.Net;

namespace PictureLibrary.API
{
    public static class ExceptionHandler
    {
        public static async Task Handle(HttpContext context)
        {
            Exception? ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (ex != null) 
            {
                var errorDetails = GetErrorDetails(ex);
                context.Response.StatusCode = (int)errorDetails.StatusCode;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }

        private static ErrorResponse GetErrorDetails(Exception ex) 
        { 
            if (ex is ResourceAlreadyExistsException)
            {
                return new ErrorResponse()
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Title = ex.Message,
                };
            }
            else if (ex is NotFoundException)
            {
                return new ErrorResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Title = ex.Message,
                };
            }
            else if (ex is InvalidTokenException)
            {
                return new ErrorResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Title = "Invalid token"
                };
            }
            else
            {
                return new ErrorResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Title = "Unexcpected error occured.",
                };
            }
        }
    }
}
