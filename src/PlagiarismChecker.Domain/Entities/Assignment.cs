namespace PlagiarismChecker.Domain.Entities;

public sealed class Assignment
{
    private readonly HashSet<AssignmentFile> _assignmentFiles;

    private Assignment()
    {
        _assignmentFiles = new HashSet<AssignmentFile>();
    }

    public static Assignment Create(string name, UserId ownerId)
    {
        return new Assignment
        {
            Id = AssignmentId.New(),
            CreationTimestamp = DateTimeOffset.UtcNow,
            Name = name,
            OwnerId = ownerId
        };
    }

    public AssignmentId Id { get; private set; }
    public User Owner { get; private set; }
    public UserId OwnerId { get; private set; }
    public string Name { get; private set; }
    public DateTimeOffset CreationTimestamp { get; private set; }

    public ICollection<AssignmentFile> AssignmentFiles => _assignmentFiles;
}