using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Common.Services;
using PlagiarismChecker.Core.Mappers;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Core.Student.Exceptions;
using PlagiarismChecker.Core.Student.Options;

namespace PlagiarismChecker.Core.Student.Commands.UploadAssignmentFile;

public sealed class UploadAssignmentFileCommandHandler
    : ICommandHandler<UploadAssignmentFileCommand, AssignmentFileDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBlobService _blobService;
    private readonly IDocumentInitializationService _documentInitializationService;
    private readonly IAssignmentFileMapper _assignmentFileMapper;

    public UploadAssignmentFileCommandHandler(
        IApplicationDbContext dbContext,
        IBlobService blobService,
        IDocumentInitializationService documentInitializationService,
        IAssignmentFileMapper assignmentFileMapper
    )
    {
        _dbContext = dbContext;
        _blobService = blobService;
        _documentInitializationService = documentInitializationService;
        _assignmentFileMapper = assignmentFileMapper;
    }

    public async ValueTask<AssignmentFileDto> Handle(
        UploadAssignmentFileCommand command,
        CancellationToken cancellationToken
    )
    {
        var assignment = await _dbContext.StudentAssignments
            .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment is null)
            throw new AssignmentNotFoundException();

        if (assignment.OwnerId != command.User.GetUserId())
            throw new AssignmentAccessDeniedException();

        var blobId = await _blobService.UploadAsync(command.FileStream, command.ContentType, cancellationToken);

        var document = _documentInitializationService.Create(command.FileStream, command.ContentType, command.FileName);
        var newFile = AssignmentFile.Create(command.FileName, document, assignment, blobId);

        _dbContext.AssignmentFiles.Add(newFile);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return _assignmentFileMapper.ToDto(newFile);
    }
}