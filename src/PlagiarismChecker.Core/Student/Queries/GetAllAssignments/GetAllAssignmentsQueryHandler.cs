using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Core.Mappers;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Core.Student.Queries.GetAllAssignments;

public sealed class GetAllAssignmentsQueryHandler : IQueryHandler<GetAllAssignmentsQuery, IEnumerable<AssignmentDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAssignmentMapper _assignmentMapper;

    public GetAllAssignmentsQueryHandler(IApplicationDbContext dbContext, IAssignmentMapper assignmentMapper)
    {
        _dbContext = dbContext;
        _assignmentMapper = assignmentMapper;
    }

    public async ValueTask<IEnumerable<AssignmentDto>> Handle(
        GetAllAssignmentsQuery query,
        CancellationToken cancellationToken
    )
    {
        var userId = query.User.GetUserId();

        var assignment = await _dbContext
            .StudentAssignments
            .AsNoTracking()
            .Include(a => a.AssignmentFiles)
            .Where(a => a.OwnerId == userId)
            .ToArrayAsync(cancellationToken);

        return _assignmentMapper.ToEnumerableDto(assignment);
    }
}