using Mediator;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Common.Services;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Core.Admin.Commands.UploadTrustedFile;

public sealed class UploadBaseFileCommandHandler
    : ICommandHandler<UploadBaseFileCommand, UploadBaseFileCommandResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBlobService _blobService;
    private readonly IDocumentInitializationService _documentInitializationService;

    public UploadBaseFileCommandHandler(
        IApplicationDbContext dbContext,
        IBlobService blobService,
        IDocumentInitializationService documentInitializationService
    )
    {
        _dbContext = dbContext;
        _blobService = blobService;
        _documentInitializationService = documentInitializationService;
    }

    public async ValueTask<UploadBaseFileCommandResult> Handle(
        UploadBaseFileCommand command,
        CancellationToken cancellationToken
    )
    {
        var blobId = await _blobService.UploadAsync(command.FileStream, command.ContentType, cancellationToken);

        var newFile = new BaseFile
        {
            Id = BaseFileId.New(),
            FileName = command.FileName,
            BlobFileId = blobId
        };

        _dbContext.BaseFiles.Add(newFile);

        var document = _documentInitializationService.Create(command.FileStream, command.ContentType, command.FileName);
        _dbContext.Documents.Add(document);

        newFile.DocumentId = document.Id;

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return new UploadBaseFileCommandResult
        {
            Id = newFile.Id,
            Name = newFile.FileName
        };
    }
}