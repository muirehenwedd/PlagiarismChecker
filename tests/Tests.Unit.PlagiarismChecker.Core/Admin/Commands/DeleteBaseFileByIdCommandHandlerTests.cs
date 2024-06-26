﻿using Bogus;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using NSubstitute;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Admin.Commands.DeleteBaseFileById;
using PlagiarismChecker.Core.Admin.Exceptions;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;
using PlagiarismChecker.Infrastructure.Data;
using Tests.Unit.PlagiarismChecker.Core.__Utils;

namespace Tests.Unit.PlagiarismChecker.Core.Admin.Commands;

public sealed class DeleteBaseFileByIdCommandHandlerTests
{
    private readonly Faker<BaseFile> _baseFileFaker;
    private readonly DeleteBaseFileByIdCommandHandler _sut;
    private readonly IApplicationDbContext _dbContextSubstitute;
    private readonly IBlobService _blobService;

    public DeleteBaseFileByIdCommandHandlerTests()
    {
        _baseFileFaker = BaseFileFaker.Create();
        _dbContextSubstitute = Create.MockedDbContextFor<ApplicationDbContext>();
        _blobService = Substitute.For<IBlobService>();

        _sut = new(_dbContextSubstitute, _blobService);
    }

    [Fact]
    public async Task Handle_ThrowsWhenFileNotFound()
    {
        // Arrange
        var generated = _baseFileFaker.Generate();
        var command = new DeleteBaseFileByIdCommand(generated.Id);

        // Act
        var act = async () => { await _sut.Handle(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<BaseFileNotFoundException>().WithMessage("File was not found.");
    }

    [Fact]
    public async Task Handle_CallsFileServiceProperty()
    {
        // Arrange
        var generated = _baseFileFaker.Generate();
        _dbContextSubstitute.BaseFiles.Add(generated);
        await _dbContextSubstitute.SaveChangesAsync();
        _dbContextSubstitute.ClearReceivedCalls();

        var cancellationToken = new CancellationTokenSource().Token;

        var command = new DeleteBaseFileByIdCommand(generated.Id);

        // Act
        _ = await _sut.Handle(command, cancellationToken);

        // Assert
        _dbContextSubstitute.BaseFiles.Received(1).Remove(generated);
        await _dbContextSubstitute.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}