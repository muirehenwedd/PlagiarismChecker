using PlagiarismChecker.Core.Options.Abstractions;

namespace PlagiarismChecker.Infrastructure.Options;

public sealed class ConnectionStringsOptions : IBindableOptions
{
    public static string SectionPath => "ConnectionStrings";

    public string Postgres { get; set; }
    public string BlobStorage { get; set; }
}