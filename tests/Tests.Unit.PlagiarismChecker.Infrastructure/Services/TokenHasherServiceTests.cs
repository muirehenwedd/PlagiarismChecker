using FluentAssertions;
using PlagiarismChecker.Infrastructure.Services;

namespace Tests.Unit.PlagiarismChecker.Infrastructure.Services;

public sealed class TokenHasherServiceTests
{
    private readonly TokenHasherService _sut;

    public TokenHasherServiceTests()
    {
        _sut = new TokenHasherService();
    }

    [Fact]
    public void GetDocumentOrderedHashes_ShouldReturnProperResult()
    {
        // Arrange
        string[] tokens = ["word1", "word2"];
        long[] expectedTokenHashes = [5861610460711951915L, -355452524944164013L];

        // Act
        var actualTokenHashes = _sut.HashTokens(tokens);

        // Assert
        actualTokenHashes.Should().Equal(expectedTokenHashes);
    }

    [Fact]
    public void GetDocumentOrderedHashes_ShouldReturnSameResult_IfCalledMultipleTimes()
    {
        // Arrange
        string[] tokens = ["word1"];
        long[] expectedTokenHashes = [5861610460711951915L];

        // Act

        var actual1 = _sut.HashTokens(tokens);
        var actual2 = _sut.HashTokens(tokens);
        
        // Assert
        expectedTokenHashes
            .Should().Equal(actual1)
            .And.Equal(actual2);
    }
}