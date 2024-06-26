﻿using System.Net.Mime;
using FluentAssertions;
using NSubstitute;
using PlagiarismChecker.Core.Services;
using PlagiarismChecker.Infrastructure.Services;

namespace Tests.Unit.PlagiarismChecker.Infrastructure.Services;

public sealed class DocumentInitializationServiceTests
{
    private readonly DocumentInitializationService _sut;
    private readonly IFileReaderService _fileReaderService;
    private readonly ITokenizerService _tokenizerService;
    private readonly ITokenHasherService _tokenHasherService;
    private readonly IHashSorterService _hashSorterService;

    public DocumentInitializationServiceTests()
    {
        _fileReaderService = Substitute.For<IFileReaderService>();
        _tokenizerService = Substitute.For<ITokenizerService>();
        _tokenHasherService = Substitute.For<ITokenHasherService>();
        _hashSorterService = Substitute.For<IHashSorterService>();

        _sut = new DocumentInitializationService(
            _fileReaderService,
            _tokenizerService,
            _tokenHasherService,
            _hashSorterService
        );
    }

    [Fact]
    public void Create_ShouldAssignProperFirstFileIndex()
    {
        // Arrange
        var stream = Stream.Null;
        var contentType = MediaTypeNames.Text.Plain;
        var fileName = "mock";

        _fileReaderService.ReadFile(stream, contentType).Returns(string.Empty);

        string[] tokens = ["qqq", "qq", "word"]; // length > 3 word at index 2.
        _tokenizerService.SplitWords(string.Empty).Returns(tokens);

        _tokenHasherService.HashTokens(tokens).Returns([]);

        // Act
        var actualResult = _sut.Create(stream, contentType, fileName);

        // Assert
        actualResult.FirstWordIndex.Should().Be(2);
    }
}