using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Presentation.Authentication;

namespace Presentation.Authorization.Policies;

public sealed class UserPolicy : AuthorizationPolicy
{
    public const string Name = "User";

    private static readonly IEnumerable<IAuthorizationRequirement> requirements = new[]
    {
        new DenyAnonymousAuthorizationRequirement()
    };

    private static readonly IEnumerable<string> authenticationSchemes = new[]
    {
        JwtAuth.SchemeName
    };

    public UserPolicy() : base(requirements, authenticationSchemes)
    {
    }
}