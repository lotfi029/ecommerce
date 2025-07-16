using System.Security.Claims;

namespace eCommerceCatalogService.API.Extensions;

public static class GetUserIdExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
