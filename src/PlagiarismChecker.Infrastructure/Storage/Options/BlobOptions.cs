using PlagiarismChecker.Core.Options.Abstractions;

namespace PlagiarismChecker.Infrastructure.Storage.Options;

public sealed class BlobOptions : IBindableOptions
{
    public static string SectionPath => "BlobStorage";

    public string ContainerName { get; set; }
}