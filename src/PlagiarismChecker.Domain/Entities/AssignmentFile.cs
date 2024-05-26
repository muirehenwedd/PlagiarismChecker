namespace PlagiarismChecker.Domain.Entities;

public sealed class AssignmentFile
{
    public AssignmentFileId Id { get; set; }
    public required string FileName { get; set; }
    public required BlobFileId BlobFileId { get; set; }

    public Assignment Assignment { get; set; }
    public AssignmentId AssignmentId { get; set; }
    public DocumentId DocumentId { get; set; }
    public Document Document { get; set; }
}