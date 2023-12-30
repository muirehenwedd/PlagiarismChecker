using System.Text;
using PlagiarismChecker.Core.Options.Abstractions;

namespace PlagiarismChecker.Infrastructure.Options;

public sealed class JwtOptions : IBindableOptions
{
    public static string SectionPath => "Auth:Jwt";

    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int TtlSeconds { get; set; }
    public byte[] SecretBytes => Encoding.ASCII.GetBytes(Secret);
}