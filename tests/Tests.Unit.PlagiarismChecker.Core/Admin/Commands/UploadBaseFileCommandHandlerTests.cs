using System.Net.Mime;
using Bogus;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Admin.Commands.UploadTrustedFile;
using PlagiarismChecker.Core.Common.Services;
using PlagiarismChecker.Core.Student.Options;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;
using PlagiarismChecker.Infrastructure.Data;

namespace Tests.Unit.PlagiarismChecker.Core.Admin.Commands;

public class UploadBaseFileCommandHandlerTests
{
    private readonly Faker<BaseFile> _baseFileFaker = new Faker<BaseFile>()
        .RuleFor(x => x.Id, x => BaseFileId.New())
        .RuleFor(x => x.FileName, x => x.System.FileName("txt"))
        .UseSeed(8);

    private readonly UploadBaseFileCommandHandler _sut;
    private readonly IApplicationDbContext _dbContextSubstitute;
    private readonly IBlobService _blobService;
    private readonly IDocumentInitializationService _documentInitializationService;
    private readonly Stream _fakeFileStream;

    public UploadBaseFileCommandHandlerTests()
    {
        _dbContextSubstitute = Create.MockedDbContextFor<ApplicationDbContext>();
        _documentInitializationService = Substitute.For<IDocumentInitializationService>();
        _blobService = Substitute.For<IBlobService>();

        _fakeFileStream = new MemoryStream("""
                                           This is fake text data.
                                           """u8.ToArray());

        _sut = new UploadBaseFileCommandHandler(
            _dbContextSubstitute,
            _blobService,
            _documentInitializationService
        );
    }

    [Fact]
    public async Task Handle_CallsDbContextProperly()
    {
        // Arrange
        var generated = _baseFileFaker.Generate();
        var blobFileId = Guid.NewGuid();
        _blobService.UploadAsync(_fakeFileStream, generated.FileName).Returns(Task.FromResult(blobFileId));

        var contentType = MediaTypeNames.Text.Plain;
        var command = new UploadBaseFileCommand(_fakeFileStream, contentType, generated.FileName);

        var document = new Document
        {
            WordsCount = 0,
            FirstFileIndex = 0,
            DocumentSortedWordHashes = [],
            NumericOrderedWordHashes = [],
            NumericOrderedWordIndexes = []
        };

        _documentInitializationService.Create(_fakeFileStream, contentType, generated.FileName)
            .Returns(document);

        // Act
        _ = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _dbContextSubstitute.BaseFiles.Received(1).Add(Arg.Is<BaseFile>(tf => tf.FileName == generated.FileName));
        await _dbContextSubstitute.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    /*[Fact]
    public async Task Handle_CallsFileServiceProperly()
    {
        // Arrange
        var generated = _baseFileFaker.Generate();
        _filesServiceSubstitute.WriteFileAsync(_fakeFileStream, generated.FileName).Returns(Task.CompletedTask);

        var baseFileId = Guid.NewGuid();
        _guidGenerator.NewGuid().Returns(baseFileId);

        _fileSystem.Path.GetExtension(Arg.Any<string>()).Returns(".txt");

        var command = new UploadBaseFileCommand(_fakeFileStream, generated.FileName);

        // Act
        _ = await _sut.Handle(command, CancellationToken.None);

        // Assert
        await _filesServiceSubstitute.Received(1).WriteFileAsync(_fakeFileStream, $"{baseFileId}.txt");
    }*/

    [Fact]
    public async Task Handle_ReturnsProperResult()
    {
        // Arrange
        var generated = _baseFileFaker.Generate();
        var blobFileId = Guid.NewGuid();

        _blobService.UploadAsync(_fakeFileStream, generated.FileName).Returns(Task.FromResult(blobFileId));

        var contentType = MediaTypeNames.Text.Plain;

        var document = new Document
        {
            WordsCount = 0,
            FirstFileIndex = 0,
            DocumentSortedWordHashes = [],
            NumericOrderedWordHashes = [],
            NumericOrderedWordIndexes = []
        };

        _documentInitializationService.Create(_fakeFileStream, contentType, generated.FileName).Returns(document);

        var command = new UploadBaseFileCommand(_fakeFileStream, contentType, generated.FileName);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().NotBe(BaseFileId.Empty);
        result.Name.Should().Be(generated.FileName);
    }
}