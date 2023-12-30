using PlagiarismChecker.Core.Mappers;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace PlagiarismChecker.Infrastructure.Mappers;

[Mapper]
public partial class AssignmentMapper : IAssignmentMapper
{
    private readonly IAssignmentFileMapper _assignmentFileMapper;

    public AssignmentMapper(IAssignmentFileMapper assignmentFileMapper)
    {
        _assignmentFileMapper = assignmentFileMapper;
    }

    [MapProperty(nameof(StudentAssignment.Name), nameof(AssignmentDto.AssignmentName))]
    public partial AssignmentDto ToDto(StudentAssignment from);

    public partial IEnumerable<AssignmentDto> ToEnumerableDto(IEnumerable<StudentAssignment> from);

    public partial IQueryable<AssignmentDto> ToQueryableDto(IQueryable<StudentAssignment> from);

    private AssignmentFileDto ToDto(AssignmentFile from)
    {
        return _assignmentFileMapper.ToDto(from);
    }
}