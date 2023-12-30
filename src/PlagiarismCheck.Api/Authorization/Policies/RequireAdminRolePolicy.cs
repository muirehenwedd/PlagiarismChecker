using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PlagiarismCheck.Api.Authentication;

namespace PlagiarismCheck.Api.Authorization.Policies;

public sealed class RequireAdminRolePolicy : AuthorizationPolicy
{
    public const string Name = "Admin";

    public RequireAdminRolePolicy() : base(
        [
            new DenyAnonymousAuthorizationRequirement(),
            new RolesAuthorizationRequirement(new[] {AuthorizationConstants.AdminRoleName})
        ],
        [
            ApikeyAuth.SchemeName
        ])
    {
    }
}