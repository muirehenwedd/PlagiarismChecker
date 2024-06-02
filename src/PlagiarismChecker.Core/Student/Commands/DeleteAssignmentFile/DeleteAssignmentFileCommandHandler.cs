using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Core.Student.Exceptions;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Core.Student.Commands.DeleteAssignmentFile;

public sealed class DeleteAssignmentFileCommandHandler : ICommandHandler<DeleteAssignmentFileCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteAssignmentFileCommandHandler(IApplicationDbContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async ValueTask<Unit> Handle(DeleteAssignmentFileCommand command, CancellationToken cancellationToken)
    {
        var assignment = await _dbContext
            .Assignments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment is null)
            throw new AssignmentNotFoundException();

        if (assignment.OwnerId != command.User.GetUserId())
            throw new AssignmentAccessDeniedException();

        var assignmentFile = await _dbContext
            .AssignmentFiles
            .Include(e => e.Document)
            .FirstOrDefaultAsync(f => f.Id == command.AssignmentFileId,
                cancellationToken);

        if (assignmentFile is null)
            throw new AssignmentFileNotFoundException();

        await _blobService.DeleteAsync(assignmentFile.BlobFileId, cancellationToken);

        _dbContext.AssignmentFiles.Remove(assignmentFile);
        _dbContext.Documents.Remove(assignmentFile.Document);

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return Unit.Value;
    }
}