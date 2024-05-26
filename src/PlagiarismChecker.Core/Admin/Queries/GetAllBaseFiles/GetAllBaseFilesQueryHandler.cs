using Mediator;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Core.Admin.DTOs;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Core.Admin.Queries.GetAllTrustedFiles;

public sealed class GetAllBaseFilesQueryHandler
    : IQueryHandler<GetAllBaseFilesQuery, GetAllBaseFilesQueryResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllBaseFilesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<GetAllBaseFilesQueryResult> Handle(
        GetAllBaseFilesQuery query,
        CancellationToken cancellationToken
    )
    {
        var files = await _dbContext
            .BaseFiles
            .AsNoTracking()
            .Select(file => new BaseFileDto
            {
                Id = file.Id,
                Name = file.Name
            })
            .ToArrayAsync(cancellationToken);

        return new GetAllBaseFilesQueryResult(files);
    }
}