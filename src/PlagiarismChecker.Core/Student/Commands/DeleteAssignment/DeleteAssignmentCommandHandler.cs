using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Domain.Repository;
using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Student.Exceptions;

namespace PlagiarismChecker.Core.Student.Commands.DeleteAssignment;

public sealed class DeleteAssignmentCommandHandler : ICommandHandler<DeleteAssignmentCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteAssignmentCommandHandler(IApplicationDbContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async ValueTask<Unit> Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken)
    {
        var assignment = await _dbContext.StudentAssignments
            .Include(a => a.AssignmentFiles)
            .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment is null)
            throw new AssignmentNotFoundException();

        if (assignment.OwnerId != command.User.GetUserId())
            throw new AssignmentAccessDeniedException();

        foreach (var assignmentFile in assignment.AssignmentFiles)
        {
            await _blobService.DeleteAsync(assignmentFile.BlobFileId, CancellationToken.None);
        }

        _dbContext.AssignmentFiles.RemoveRange(assignment.AssignmentFiles);
        _dbContext.StudentAssignments.Remove(assignment);

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return Unit.Value;
    }
}