namespace PlagiarismChecker.Core.Admin.DTOs;

public class BaseFileDto
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
}