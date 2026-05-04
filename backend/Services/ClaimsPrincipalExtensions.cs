using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SpaceDC.Models.Enums;

namespace SpaceDC.Services;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (sub is null || !Guid.TryParse(sub, out var id))
            throw new InvalidOperationException("User id claim is missing or invalid.");
        return id;
    }

    public static UserRole GetUserRole(this ClaimsPrincipal user)
    {
        var role = user.FindFirstValue(ClaimTypes.Role);
        if (role is null || !Enum.TryParse<UserRole>(role, ignoreCase: true, out var parsed))
            throw new InvalidOperationException("User role claim is missing or invalid.");
        return parsed;
    }
}
