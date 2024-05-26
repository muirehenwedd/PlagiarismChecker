using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Admin.DTOs;

public class BaseFileDto
{
    public required BaseFileId Id { get; init; }
    public required string Name { get; set; }
}