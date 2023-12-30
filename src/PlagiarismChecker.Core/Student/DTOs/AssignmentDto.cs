namespace PlagiarismChecker.Core.Student.DTOs;

public class AssignmentDto
{
    public Guid Id { get; init; }
    public string AssignmentName { get; init; }
    public DateTimeOffset CreationTimestamp { get; init; }
    public IEnumerable<AssignmentFileDto> AssignmentFiles { get; init; }
}