namespace PlagiarismChecker.Domain.Entities;

public sealed class AssignmentFile
{
    private AssignmentFile()
    {
    }

    public static AssignmentFile Create(string name, Document document, Assignment assignment, BlobFileId blobFileId)
    {
        return new AssignmentFile
        {
            Id = AssignmentFileId.New(),
            Document = document,
            DocumentId = document.Id,
            Name = name,
            Assignment = assignment,
            AssignmentId = assignment.Id,
            BlobFileId = blobFileId
        };
    }

    public AssignmentFileId Id { get; private set; }
    public string Name { get; private set; }
    public BlobFileId BlobFileId { get; private set; }

    public Assignment Assignment { get; private set; }
    public AssignmentId AssignmentId { get; private set; }
    public DocumentId DocumentId { get; private set; }
    public Document Document { get; private set; }
}