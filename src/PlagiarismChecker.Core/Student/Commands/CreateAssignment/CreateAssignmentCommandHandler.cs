using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;
using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Mappers;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Core.Student.Exceptions;

namespace PlagiarismChecker.Core.Student.Commands.CreateAssignment;

public sealed class CreateAssignmentCommandHandler
    : ICommandHandler<CreateAssignmentCommand, AssignmentDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly TimeProvider _timeProvider;
    private readonly IAssignmentMapper _assignmentMapper;

    public CreateAssignmentCommandHandler(
        IApplicationDbContext dbContext,
        TimeProvider timeProvider,
        IAssignmentMapper assignmentMapper
    )
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
        _assignmentMapper = assignmentMapper;
    }

    public async ValueTask<AssignmentDto> Handle(
        CreateAssignmentCommand command,
        CancellationToken cancellationToken
    )
    {
        var userId = command.User.GetUserId();

        var notUnique = await _dbContext.StudentAssignments
            .AnyAsync(a => a.OwnerId == userId && a.Name == command.Name, cancellationToken);

        if (notUnique)
            throw new AssignmentAlreadyCreatedException();

        var assignment = Assignment.Create(command.Name, userId);

        _dbContext.StudentAssignments.Add(assignment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _assignmentMapper.ToDto(assignment);
    }
}