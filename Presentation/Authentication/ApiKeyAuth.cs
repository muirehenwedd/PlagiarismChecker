using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Presentation.Authentication;

public sealed class ApikeyAuth : AuthenticationHandler<AuthenticationSchemeOptions>
{
    internal const string SchemeName = "ApiKey";
    private const string HeaderName = "x-api-key";
    private string _apiKey;

    private static readonly AuthenticationTicket CachedTicket = new(
        new GenericPrincipal(
            new ClaimsIdentity(
                claims: null,
                authenticationType: SchemeName), roles: new[] {"Admin"}
        ), SchemeName);

    private static readonly Task<AuthenticateResult> CachedNoKeyTask =
        Task.FromResult(AuthenticateResult.Fail("Required header is not present."));

    private static readonly Task<AuthenticateResult> CachedHeaderEmptyTask =
        Task.FromResult(AuthenticateResult.Fail("Required has no value."));

    private static readonly Task<AuthenticateResult> CachedInvalidKeyTask =
        Task.FromResult(AuthenticateResult.Fail("Invalid API key."));

    private static readonly Task<AuthenticateResult> CachedSuccessfulTask =
        Task.FromResult(AuthenticateResult.Success(CachedTicket));

    public ApikeyAuth(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptionsMonitor<AuthOptions> config
    ) : base(options, logger, encoder)
    {
        _apiKey = config.CurrentValue.ApiKey;
        config.OnChange(authOptions => Interlocked.Exchange(ref _apiKey, authOptions.ApiKey));
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Headers.TryGetValue(HeaderName, out var extractedApiKey) is false)
            return CachedNoKeyTask;

        if (extractedApiKey.Count == 0)
            return CachedHeaderEmptyTask;

        return _apiKey.Equals(extractedApiKey[0]) ? CachedSuccessfulTask : CachedInvalidKeyTask;
    }
}