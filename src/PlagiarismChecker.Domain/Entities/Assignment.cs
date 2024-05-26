namespace PlagiarismChecker.Domain.Entities;

public sealed class Assignment
{
    public AssignmentId Id { get; set; }
    public User Owner { get; set; }
    public UserId OwnerId { get; set; }
    public required string Name { get; set; }
    public required DateTimeOffset CreationTimestamp { get; set; }

    public ICollection<AssignmentFile> AssignmentFiles { get; set; }
}