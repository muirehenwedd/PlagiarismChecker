using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Domain.Repository;
using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Mappers;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Core.Student.Exceptions;

namespace PlagiarismChecker.Core.Student.Queries.GetAssignment;

public sealed class GetAssignmentQueryHandler : IQueryHandler<GetAssignmentQuery, AssignmentDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAssignmentMapper _assignmentMapper;

    public GetAssignmentQueryHandler(IApplicationDbContext dbContext, IAssignmentMapper assignmentMapper)
    {
        _dbContext = dbContext;
        _assignmentMapper = assignmentMapper;
    }

    public async ValueTask<AssignmentDto> Handle(
        GetAssignmentQuery query,
        CancellationToken cancellationToken
    )
    {
        var assignment = await _dbContext
            .StudentAssignments
            .AsNoTracking()
            .Include(a => a.AssignmentFiles)
            .FirstOrDefaultAsync(a => a.Id == query.AssignmentId, cancellationToken);

        if (assignment is null)
            throw new AssignmentNotFoundException();

        if (assignment.OwnerId != query.User.GetUserId())
            throw new AssignmentAccessDeniedException();

        return _assignmentMapper.ToDto(assignment);
    }
}