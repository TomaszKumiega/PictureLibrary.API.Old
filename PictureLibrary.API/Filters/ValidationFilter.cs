using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PictureLibrary.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Any() == true)
                    .ToDictionary(
                        pair => pair.Key, 
                        pair => pair.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

                var errorResponse = new ErrorResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Title = "Validation problems",
                    Details = new List<ErrorDetails>(),
                };

                foreach (var error in errors)
                {
                    foreach (var subError in error.Value!)
                    {
                        var validationErrorDetails = new ErrorDetails(error.Key, subError);
                        errorResponse.Details.Add(validationErrorDetails);
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
            }

            await next();
        }
    }
}
