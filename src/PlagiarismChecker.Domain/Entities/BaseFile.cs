namespace PlagiarismChecker.Domain.Entities;

public sealed class BaseFile
{
    public Guid Id { get; set; }
    public required string FileName { get; set; }
    public Guid DocumentId { get; set; }
    public Document Document { get; set; }
    public Guid BlobFileId { get; set; }
}