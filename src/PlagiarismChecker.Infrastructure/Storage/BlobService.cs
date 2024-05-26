using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Infrastructure.Storage.Options;

namespace PlagiarismChecker.Infrastructure.Storage;

internal sealed class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IOptions<BlobOptions> _options;

    public BlobService(BlobServiceClient blobServiceClient, IOptions<BlobOptions> options)
    {
        _blobServiceClient = blobServiceClient;
        _options = options;
    }

    public async Task<BlobFileId> UploadAsync(
        Stream stream,
        string contentType,
        CancellationToken cancellationToken = default
    )
    {
        var fileId = BlobFileId.New();

        var blobClient = GetBlobClient(fileId);

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders {ContentType = contentType},
            cancellationToken: cancellationToken);

        return fileId;
    }

    public async Task<FileResponse> DownloadAsync(BlobFileId fileId, CancellationToken cancellationToken = default)
    {
        var blobClient = GetBlobClient(fileId);

        var response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task DeleteAsync(BlobFileId fileId, CancellationToken cancellationToken = default)
    {
        var blobClient = GetBlobClient(fileId);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    private BlobClient GetBlobClient(BlobFileId blobId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_options.Value.ContainerName);
        var blobClient = containerClient.GetBlobClient(blobId.ToString());

        return blobClient;
    }
}