using Microsoft.AspNetCore.Diagnostics;
using PictureLibrary.DataAccess.Exceptions;
using System.Net;

namespace PictureLibrary.API.ExceptionHandler
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

        private static ErrorDetails GetErrorDetails(Exception ex) 
        { 
            if (ex is ResourceAlreadyExistsException)
            {
                return new ErrorDetails()
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Title = ex.Message,
                };
            }
            else if (ex is NotFoundException)
            {
                return new ErrorDetails()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Title = ex.Message,
                };
            }
            else
            {
                return new ErrorDetails()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Title = "Unexcpected error occured.",
                };
            }
        }
    }
}
