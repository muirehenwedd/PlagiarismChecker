using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Infrastructure.Options;

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
            var options = provider.GetRequiredService<IOptions<ConnectionStringsOptions>>();

            var client = new BlobServiceClient(options.Value.BlobStorage);
            CreateBlobContainerIfNotExist(client);

            return client;
        });

        return services;
    }

    private static void CreateBlobContainerIfNotExist(BlobServiceClient client)
    {
        var containerClient = client.GetBlobContainerClient(BlobService.ContainerName);
        containerClient.CreateIfNotExists();
    }
}