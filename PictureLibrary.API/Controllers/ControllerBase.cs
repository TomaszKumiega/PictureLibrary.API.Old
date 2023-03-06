using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PictureLibrary.API.Controllers
{
    public class ControllerBase : Controller
    {
        protected bool IsUserAuthorized(Guid userId)
        {
            Guid? currentUserId = GetCurrentUserId();
            return currentUserId.HasValue && currentUserId == userId;
        }

        protected Guid? GetCurrentUserId()
        {
            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out Guid currentUserId)
                ? currentUserId
                : null;
        }
    }
}
