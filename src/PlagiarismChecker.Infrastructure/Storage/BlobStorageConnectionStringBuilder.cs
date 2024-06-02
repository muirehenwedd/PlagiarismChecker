using Microsoft.Extensions.Options;
using PlagiarismChecker.Infrastructure.Storage.Options;

namespace PlagiarismChecker.Infrastructure.Storage;

public sealed class BlobStorageConnectionStringBuilder : IBlobStorageConnectionStringBuilder
{
    private readonly IOptions<BlobOptions> _options;

    public BlobStorageConnectionStringBuilder(IOptions<BlobOptions> options)
    {
        _options = options;
    }

    public string GetConnectionString()
    {
        return $"UseDevelopmentStorage=true;DevelopmentStorageProxyUri={_options.Value.ServiceUri}";
    }
}

public interface IBlobStorageConnectionStringBuilder
{
    string GetConnectionString();
}