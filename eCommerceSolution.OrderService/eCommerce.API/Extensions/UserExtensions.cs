using System.Security.Claims;

namespace eCommerce.API.Extensions;

public static class UserExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.Parse(userId!);
    }
}