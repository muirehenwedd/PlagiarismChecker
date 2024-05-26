using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Abstractions.Storage;

public interface IBlobService
{
    Task<BlobFileId> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<FileResponse> DownloadAsync(BlobFileId fileId, CancellationToken cancellationToken = default);
    Task DeleteAsync(BlobFileId fileId, CancellationToken cancellationToken = default);
}