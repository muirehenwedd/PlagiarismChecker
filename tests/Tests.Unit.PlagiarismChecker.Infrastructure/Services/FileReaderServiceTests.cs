using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using FluentAssertions;
using PlagiarismChecker.Infrastructure.Services;

namespace Tests.Unit.PlagiarismChecker.Infrastructure.Services;

public sealed class FileReaderServiceTests
{
    private readonly FileReaderService _sut;

    public FileReaderServiceTests()
    {
        _sut = new FileReaderService();
    }

    [Fact]
    public void ReadFile_ShouldThrowUnreachableException_IfMediaTypeIsNotSupported()
    {
        // Arrange
        var stream = Stream.Null;
        var contentType = MediaTypeNames.Image.Gif;

        // Act
        var invocationAction = () => _sut.ReadFile(stream, contentType);

        // Assert
        invocationAction.Should().ThrowExactly<UnreachableException>();
    }

    [Fact]
    public void ReadFile_ReadsTxtFileProperly()
    {
        // Arrange
        var text = "lorem ipsum";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        var mediaType = MediaTypeNames.Text.Plain;

        // Act
        var actualText = _sut.ReadFile(stream, mediaType);

        // Assert
        actualText.Should().Be(text);
    }
}