using PlagiarismCheck.Api.Authorization.Policies;

namespace PlagiarismCheck.Api.Authorization.Configuration;

public static class Extensions
{
    public static IServiceCollection RegisterAuthorization(this IServiceCollection collection)
    {
        collection.AddAuthorizationBuilder()
            .AddPolicy(RequireAdminRolePolicy.Name, new RequireAdminRolePolicy())
            .AddPolicy(RequireStudentRolePolicy.Name, new RequireStudentRolePolicy());

        return collection;
    }
}