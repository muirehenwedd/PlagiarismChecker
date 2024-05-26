using System.Security.Claims;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Common.Extensions;

public static class ClaimsPrincipalException
{
    public static UserId GetUserId(this ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            throw new InvalidOperationException();

        return new UserId(Guid.Parse(userIdClaim));
    }
}