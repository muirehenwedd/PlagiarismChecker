using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Presentation.Authentication;

namespace Presentation.Authorization.Policies;

public sealed class AdminPolicy : AuthorizationPolicy
{
    public const string Name = "Admin";

    private static readonly IEnumerable<IAuthorizationRequirement> requirements = new[]
    {
        new DenyAnonymousAuthorizationRequirement()
    };

    private static readonly IEnumerable<string> authenticationSchemes = new[]
    {
        ApikeyAuth.SchemeName
    };

    public AdminPolicy() : base(requirements, authenticationSchemes)
    {
    }
}