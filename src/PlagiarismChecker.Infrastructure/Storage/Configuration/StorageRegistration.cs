using System.Reflection.Metadata;
using Azure.Core.Pipeline;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Infrastructure.Storage.Options;

namespace PlagiarismChecker.Infrastructure.Storage.Configuration;

public static class StorageRegistration
{
    public static IServiceCollection RegisterStorage(
        this IServiceCollection services
    )
    {
        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton<IBlobStorageConnectionStringBuilder, BlobStorageConnectionStringBuilder>();
        services.AddSingleton(provider =>
        {
            var connectionStringBuilder = provider.GetRequiredService<IBlobStorageConnectionStringBuilder>();
            var connectionString = connectionStringBuilder.GetConnectionString();

            var client = new BlobServiceClient(connectionString);

            var blobOptions = provider.GetRequiredService<IOptions<BlobOptions>>();
            CreateBlobContainerIfNotExist(client, blobOptions.Value.ContainerName);

            return client;
        });

        return services;
    }

    private static void CreateBlobContainerIfNotExist(BlobServiceClient client, string containerName)
    {
        var containerClient = client.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();
    }
}