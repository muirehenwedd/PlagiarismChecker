using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Admin.Exceptions;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Core.Admin.Commands.DeleteTrustedFileByName;

public sealed class DeleteBaseFileByNameCommandHandler : ICommandHandler<DeleteBaseFileByNameCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteBaseFileByNameCommandHandler(IApplicationDbContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async ValueTask<Unit> Handle(DeleteBaseFileByNameCommand command, CancellationToken cancellationToken)
    {
        var file = await _dbContext
            .BaseFiles
            .Include(e => e.Document)
            .FirstOrDefaultAsync(f => f.Name == command.Name,
                cancellationToken);

        if (file is null)
            throw new BaseFileNotFoundException();

        await _blobService.DeleteAsync(file.BlobFileId, cancellationToken);

        _dbContext.BaseFiles.Remove(file);
        _dbContext.Documents.Remove(file.Document);

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return Unit.Value;
    }
}