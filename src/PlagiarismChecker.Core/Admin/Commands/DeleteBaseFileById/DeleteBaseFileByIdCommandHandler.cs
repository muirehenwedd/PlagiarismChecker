using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Admin.Exceptions;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Core.Admin.Commands.DeleteBaseFileById;

public sealed class DeleteBaseFileByIdCommandHandler : ICommandHandler<DeleteBaseFileByIdCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteBaseFileByIdCommandHandler(IApplicationDbContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async ValueTask<Unit> Handle(DeleteBaseFileByIdCommand command, CancellationToken cancellationToken)
    {
        var file = await _dbContext
            .BaseFiles
            .Include(e => e.Document)
            .FirstOrDefaultAsync(f => f.Id == command.BaseFileId,
                cancellationToken);

        if (file is null)
            throw new BaseFileNotFoundException();

        await _blobService.DeleteAsync(file.BlobFileId, CancellationToken.None);

        _dbContext.BaseFiles.Remove(file);
        _dbContext.Documents.Remove(file.Document);

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return Unit.Value;
    }
}