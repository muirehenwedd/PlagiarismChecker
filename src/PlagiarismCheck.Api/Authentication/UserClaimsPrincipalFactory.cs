using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismCheck.Api.Authentication;

public sealed class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public ClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    {
    }

    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var claims = await base.GenerateClaimsAsync(user);

        throw new NotImplementedException();
    }
}