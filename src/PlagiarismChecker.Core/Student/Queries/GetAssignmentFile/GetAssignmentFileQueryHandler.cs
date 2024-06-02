using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Core.Student.Exceptions;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Core.Student.Queries.GetAssignmentFile;

public sealed class GetAssignmentFileQueryHandler
    : IQueryHandler<GetAssignmentFileQuery, GetAssignmentFileQueryResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetAssignmentFileQueryHandler(IApplicationDbContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async ValueTask<GetAssignmentFileQueryResult> Handle(
        GetAssignmentFileQuery query,
        CancellationToken cancellationToken
    )
    {
        var assignment = await _dbContext
            .Assignments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == query.AssignmentId, cancellationToken);

        if (assignment is null)
            throw new AssignmentNotFoundException();

        if (assignment.OwnerId != query.User.GetUserId())
            throw new AssignmentAccessDeniedException();

        var assignmentFile = await _dbContext
            .AssignmentFiles
            .Select(e => new
            {
                e.Id,
                e.BlobFileId,
                FileName = e.Name
            })
            .FirstOrDefaultAsync(f => f.Id == query.AssignmentFileId, cancellationToken);

        if (assignmentFile is null)
            throw new AssignmentFileNotFoundException();

        var (stream, contentType) = await _blobService.DownloadAsync(assignmentFile.BlobFileId, cancellationToken);

        return new GetAssignmentFileQueryResult(stream, assignmentFile.FileName, contentType);
    }
}