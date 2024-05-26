namespace PlagiarismChecker.Domain.Entities;

public sealed class BaseFile
{
    public BaseFileId Id { get; set; }
    public required string FileName { get; set; }
    public DocumentId DocumentId { get; set; }
    public Document Document { get; set; }
    public Guid BlobFileId { get; set; }
}