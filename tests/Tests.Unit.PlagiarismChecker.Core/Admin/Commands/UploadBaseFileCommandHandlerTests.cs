using System.Net.Mime;
using Bogus;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using PlagiarismChecker.Core.Abstractions.Storage;
using PlagiarismChecker.Core.Admin.Commands.UploadBaseFile;
using PlagiarismChecker.Core.Services;
using PlagiarismChecker.Core.Student.Options;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;
using PlagiarismChecker.Infrastructure.Data;
using Tests.Unit.PlagiarismChecker.Core.__Utils;

namespace Tests.Unit.PlagiarismChecker.Core.Admin.Commands;

public class UploadBaseFileCommandHandlerTests
{
    private readonly Faker<BaseFile> _baseFileFaker;

    private readonly UploadBaseFileCommandHandler _sut;
    private readonly IApplicationDbContext _dbContextSubstitute;
    private readonly IBlobService _blobService;
    private readonly IDocumentInitializationService _documentInitializationService;
    private readonly Stream _fakeFileStream;

    public UploadBaseFileCommandHandlerTests()
    {
        _baseFileFaker = BaseFileFaker.Create();
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
        var blobFileId = BlobFileId.New();
        _blobService.UploadAsync(_fakeFileStream, generated.Name).Returns(Task.FromResult(blobFileId));

        var contentType = MediaTypeNames.Text.Plain;
        var command = new UploadBaseFileCommand(_fakeFileStream, contentType, generated.Name);

        var document = Document.Create([], [], [], 0);

        _documentInitializationService.Create(_fakeFileStream, contentType, generated.Name)
            .Returns(document);

        // Act
        _ = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _dbContextSubstitute.BaseFiles.Received(1).Add(Arg.Is<BaseFile>(tf => tf.Name == generated.Name));
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
        var blobFileId = BlobFileId.New();

        _blobService.UploadAsync(_fakeFileStream, generated.Name).Returns(Task.FromResult(blobFileId));

        var contentType = MediaTypeNames.Text.Plain;

        var document = Document.Create([], [], [], 0);

        _documentInitializationService.Create(_fakeFileStream, contentType, generated.Name).Returns(document);

        var command = new UploadBaseFileCommand(_fakeFileStream, contentType, generated.Name);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().NotBe(BaseFileId.Empty);
        result.Name.Should().Be(generated.Name);
    }
}