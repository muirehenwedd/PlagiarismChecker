using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PlagiarismCheck.Api.Authentication;

namespace PlagiarismCheck.Api.Authorization.Policies;

public sealed class RequireStudentRolePolicy : AuthorizationPolicy
{
    public const string Name = "Student";

    public RequireStudentRolePolicy() : base(
        [
            new DenyAnonymousAuthorizationRequirement(),
            new RolesAuthorizationRequirement(new[] {AuthorizationConstants.StudentRoleName})
        ],
        [
            JwtAuth.SchemeName
        ])
    {
    }
}