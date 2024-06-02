using Bogus;
using EntityFrameworkCore.Testing.NSubstitute;
using EntityFrameworkCore.Testing.NSubstitute.Extensions;
using FluentAssertions;
using NSubstitute;
using PlagiarismChecker.Core.Admin.Queries.GetAllBaseFiles;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;
using PlagiarismChecker.Infrastructure.Data;
using Tests.Unit.PlagiarismChecker.Core.__Utils;

namespace Tests.Unit.PlagiarismChecker.Core.Admin.Queries;

public class GetAllBaseFilesQueryHandlerTests
{
    private readonly Faker<BaseFile> _baseFileFaker;
    private readonly IApplicationDbContext _dbContextSubstitute;

    public GetAllBaseFilesQueryHandlerTests()
    {
        _baseFileFaker = BaseFileFaker.Create();
        _dbContextSubstitute = Create.MockedDbContextFor<ApplicationDbContext>();
    }

    [Fact]
    public async Task Handle_CallsDbContextProperly()
    {
        // Arrange
        var generated = _baseFileFaker.Generate(3);

        _dbContextSubstitute.BaseFiles.AddRangeToReadOnlySource(generated);

        var query = GetAllBaseFilesQuery.Instance;
        var handler = new GetAllBaseFilesQueryHandler(_dbContextSubstitute);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert
        await _dbContextSubstitute.DidNotReceiveWithAnyArgs().SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyEnumerable_WhenDbSetIsEmpty()
    {
        // Arrange
        var query = GetAllBaseFilesQuery.Instance;
        var handler = new GetAllBaseFilesQueryHandler(_dbContextSubstitute);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Files.Should().BeEmpty();
    }
}