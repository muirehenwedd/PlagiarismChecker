using PlagiarismChecker.Core.Options.Abstractions;

namespace PlagiarismChecker.Infrastructure.Options;

public sealed class AuthOptions : IBindableOptions
{
    public static string SectionPath => "Auth";
    public required string ApiKey { get; set; }
}