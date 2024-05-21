using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Infrastructure.Options;
using PlagiarismChecker.Infrastructure.Storage.Options;

namespace PlagiarismChecker.Infrastructure.Storage.Configuration;

public static class StorageRegistration
{
    public static IServiceCollection RegisterStorage(
        this IServiceCollection services
    )
    {
        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton(provider =>
        {
            var connectionStringsOptions = provider.GetRequiredService<IOptions<ConnectionStringsOptions>>();
            var blobOptions = provider.GetRequiredService<IOptions<BlobOptions>>();

            var client = new BlobServiceClient(connectionStringsOptions.Value.BlobStorage);
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