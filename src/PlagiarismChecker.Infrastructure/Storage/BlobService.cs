using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PlagiarismChecker.Core.Abstractions.Storage;

namespace PlagiarismChecker.Infrastructure.Storage;

internal sealed class BlobService : IBlobService
{
    public const string ContainerName = "files"; //todo: move to config.
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Guid> UploadAsync(
        Stream stream,
        string contentType,
        CancellationToken cancellationToken = default
    )
    {
        var fileId = Guid.NewGuid();

        var blobClient = GetBlobClient(fileId);

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders {ContentType = contentType},
            cancellationToken: cancellationToken);

        return fileId;
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var blobClient = GetBlobClient(fileId);

        var response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var blobClient = GetBlobClient(fileId);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    private BlobClient GetBlobClient(Guid blobId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(blobId.ToString());

        return blobClient;
    }
}