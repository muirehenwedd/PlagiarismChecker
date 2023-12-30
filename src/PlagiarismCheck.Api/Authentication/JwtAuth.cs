using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlagiarismCheck.Api.Authorization;
using PlagiarismChecker.Infrastructure.Options;

namespace PlagiarismCheck.Api.Authentication;

public sealed class JwtAuth : SignInAuthenticationHandler<AuthenticationSchemeOptions>
{
    //internal const string SchemeName = "Jwt";
    internal static readonly string SchemeName = IdentityConstants.BearerScheme;

    private readonly IOptionsMonitor<JwtOptions> _config;

    private TokenValidationParameters _validationParameters;

    public JwtAuth(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptionsMonitor<JwtOptions> config
    ) : base(options, logger, encoder)
    {
        _config = config;
        _validationParameters = FromOptionValue(_config.CurrentValue);

        config.OnChange(authOptions => Interlocked.Exchange(ref _validationParameters, FromOptionValue(authOptions)));
    }

    private static readonly string[] CachedRoles = [AuthorizationConstants.StudentRoleName];

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
        var expiresAt = utcNow + TimeSpan.FromSeconds(_config.CurrentValue.TtlSeconds);

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
            Expires = expiresAt.UtcDateTime,
            NotBefore = utcNow.UtcDateTime,
            IssuedAt = utcNow.UtcDateTime,
            SigningCredentials =
                new SigningCredentials(_validationParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256Signature),
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
            IssuerSigningKey = new SymmetricSecurityKey(options.SecretBytes),
            ClockSkew = TimeSpan.Zero
        };
    }
}