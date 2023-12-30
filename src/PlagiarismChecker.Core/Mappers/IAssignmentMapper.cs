using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Mappers;

public interface IAssignmentMapper
{
    AssignmentDto ToDto(StudentAssignment from);
    IEnumerable<AssignmentDto> ToEnumerableDto(IEnumerable<StudentAssignment> from);
    IQueryable<AssignmentDto> ToQueryableDto(IQueryable<StudentAssignment> from);
}