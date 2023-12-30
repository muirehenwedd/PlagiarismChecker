namespace PlagiarismChecker.Domain.Entities;

public sealed class AssignmentFile
{
    public Guid Id { get; set; }
    public required string FileName { get; set; }
    public required Guid BlobFileId { get; set; }

    public StudentAssignment Assignment { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid DocumentId { get; set; }
    public Document Document { get; set; }
}