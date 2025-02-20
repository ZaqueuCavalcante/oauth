using System.Security.Claims;

namespace OAuth.DrawApp.Extensions;

public static class UserExtensions
{
    public static Guid Id(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("sub")!);
    }

    public static bool GoogleDriveEnabled(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue("drv");
        return claim == null ? false : bool.Parse(claim);
    }
}
