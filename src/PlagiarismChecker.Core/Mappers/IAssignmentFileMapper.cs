using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Mappers;

public interface IAssignmentFileMapper
{
    AssignmentFileDto ToDto(AssignmentFile from);
}