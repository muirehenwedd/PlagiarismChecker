using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Mappers;

public interface IAssignmentMapper
{
    AssignmentDto ToDto(Assignment from);
    IEnumerable<AssignmentDto> ToEnumerableDto(IEnumerable<Assignment> from);
    IQueryable<AssignmentDto> ToQueryableDto(IQueryable<Assignment> from);
}