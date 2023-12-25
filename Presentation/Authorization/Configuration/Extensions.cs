using Presentation.Authorization.Policies;

namespace Presentation.Authorization.Configuration;

public static class Extensions
{
    public static IServiceCollection RegisterAuthorization(this IServiceCollection collection)
    {
        collection.AddAuthorizationBuilder()
            .AddPolicy(AdminPolicy.Name, new AdminPolicy())
            .AddPolicy(UserPolicy.Name, new UserPolicy());

        return collection;
    }
}