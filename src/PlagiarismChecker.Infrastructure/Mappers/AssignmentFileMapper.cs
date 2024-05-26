using PlagiarismChecker.Core.Mappers;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace PlagiarismChecker.Infrastructure.Mappers;

[Mapper]
public sealed partial class AssignmentFileMapper : IAssignmentFileMapper
{
    [MapProperty(nameof(AssignmentFile.Name), nameof(AssignmentFileDto.Name))]
    public partial AssignmentFileDto ToDto(AssignmentFile from);

    private static Guid ToGuid(AssignmentFileId id) => id.Value;
}