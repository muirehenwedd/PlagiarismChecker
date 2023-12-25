using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Authentication;

public sealed class JwtAuth : SignInAuthenticationHandler<AuthenticationSchemeOptions>
{
    internal const string SchemeName = "Jwt";

    private readonly IOptionsMonitor<JwtOptions> _config;

    private TokenValidationParameters _validationParameters;

    public JwtAuth(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptionsMonitor<JwtOptions> config
    ) : base(options, logger, encoder)
    {
        IdentityModelEventSource.ShowPII = true;
        _config = config;
        _validationParameters = FromOptionValue(_config.CurrentValue);

        config.OnChange(authOptions => Interlocked.Exchange(ref _validationParameters, FromOptionValue(authOptions)));
    }

    private static readonly string[] CachedRoles = {"Student"};

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var tokenStringValues = Request.Headers.Authorization;

        if (tokenStringValues.Count == 0)
            return AuthenticateResult.NoResult();

        var tokenString = tokenStringValues[0];

        var handler = new JwtSecurityTokenHandler();

        var tokenValidationResult = await handler.ValidateTokenAsync(tokenString, _validationParameters);

        if (!tokenValidationResult.IsValid)
            return AuthenticateResult.Fail(tokenValidationResult.Exception);

        var tokenClaims = tokenValidationResult.Claims;

        if (!tokenClaims.TryGetValue(ClaimTypes.NameIdentifier, out var userId) || userId is not string userIdString ||
            !Guid.TryParse(userIdString, out _))
            return AuthenticateResult.Fail($"No '{ClaimTypes.NameIdentifier}' claim was found.");

        var claims =
            Enumerable.Repeat(new Claim(ClaimTypes.NameIdentifier, userIdString, ClaimValueTypes.String, ClaimsIssuer),
                1);
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new GenericPrincipal(identity, CachedRoles);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName));
    }

    protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        var utcNow = TimeProvider.GetUtcNow();
        var expiresAt = utcNow + TimeSpan.FromMinutes(60);

        properties ??= new AuthenticationProperties();
        properties.IssuedUtc = utcNow;
        properties.ExpiresUtc = expiresAt;

        var handler = new JwtSecurityTokenHandler();

        var claims = new Dictionary<string, object>()
        {
            {ClaimTypes.NameIdentifier, user.FindFirst(ClaimTypes.NameIdentifier)!.Value}
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Audience = _config.CurrentValue.Audience,
            Issuer = _config.CurrentValue.Issuer,
            Expires = expiresAt.DateTime,
            NotBefore = utcNow.DateTime,
            IssuedAt = utcNow.DateTime,
            SigningCredentials =
                new SigningCredentials(_config.CurrentValue.SecurityKey, SecurityAlgorithms.HmacSha256Signature),
            Claims = claims
        };

        var jwtSecurityToken = handler.CreateJwtSecurityToken(descriptor);

        var response = new AccessTokenResponse()
        {
            AccessToken = handler.WriteToken(jwtSecurityToken),
            ExpirationTimestamp = expiresAt
        };

        await Context.Response.WriteAsJsonAsync(response);
    }

    private class AccessTokenResponse
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpirationTimestamp { get; set; }
    }

    private TokenValidationParameters FromOptionValue(JwtOptions options)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,
            ValidateAudience = true,
            ValidAudience = options.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = options.SecurityKey,
            LifetimeValidator = LifetimeValidator,
            ClockSkew = TimeSpan.Zero
        };

        bool LifetimeValidator(
            DateTime? notbefore,
            DateTime? expires,
            SecurityToken securitytoken,
            TokenValidationParameters validationparameters
        )
        {
            var utcNow = TimeProvider.GetUtcNow();
            throw new NotImplementedException();
        }
    }
}