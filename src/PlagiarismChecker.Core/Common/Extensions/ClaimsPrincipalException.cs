using System.Security.Claims;

namespace PlagiarismChecker.Core.Common.Extensions;

public static class ClaimsPrincipalException
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            throw new InvalidOperationException();

        return Guid.Parse(userIdClaim);
    }
}