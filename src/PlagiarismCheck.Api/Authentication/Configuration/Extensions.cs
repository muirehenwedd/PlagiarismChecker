using Microsoft.AspNetCore.Authentication;

namespace PlagiarismCheck.Api.Authentication.Configuration;

public static class Extensions
{
    public static IServiceCollection RegisterAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, ApikeyAuth>(ApikeyAuth.SchemeName, null)
            .AddScheme<AuthenticationSchemeOptions, JwtAuth>(JwtAuth.SchemeName, null);

        return serviceCollection;
    }
}