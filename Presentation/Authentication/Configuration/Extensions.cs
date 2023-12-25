using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Authentication.Configuration;

public static class Extensions
{
    public static IServiceCollection RegisterAuthentication(this IServiceCollection serviceCollection)
    {
        // serviceCollection
        //     .AddAuthentication()
        //     .AddScheme<AuthenticationSchemeOptions, ApikeyAuth>(ApikeyAuth.SchemeName, null)
        //     .AddScheme<AuthenticationSchemeOptions, JwtAuth>(JwtAuth.SchemeName, null);

        serviceCollection.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme)
            .AddScheme<AuthenticationSchemeOptions, ApikeyAuth>(ApikeyAuth.SchemeName, null)
            .AddScheme<AuthenticationSchemeOptions, JwtAuth>(JwtAuth.SchemeName, null);

        return serviceCollection;
    }
}