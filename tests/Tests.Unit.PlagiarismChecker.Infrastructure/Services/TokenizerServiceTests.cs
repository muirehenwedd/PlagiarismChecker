using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using PlagiarismChecker.Core.Options;
using PlagiarismChecker.Infrastructure.Options;
using PlagiarismChecker.Infrastructure.Services;

namespace Tests.Unit.PlagiarismChecker.Infrastructure.Services;

public sealed class TokenizerServiceTests
{
    private readonly TokenizerService _sut;
    private readonly IOptions<PlagiarismCheckOptions> _options;

    public TokenizerServiceTests()
    {
        _options = Substitute.For<IOptions<PlagiarismCheckOptions>>();
        _sut = new TokenizerService(_options);
    }

    [Fact]
    public void SplitWords_ShouldSplitTextProperly()
    {
        // Arrange
        var optionsValue = new PlagiarismCheckOptions
        {
            IgnoreNumbers = false,
            IgnoreCase = false
        };

        _options.Value.Returns(optionsValue);

        var textToSplit = "word1 word2";

        // Act

        var actualTokens = _sut.SplitWords(textToSplit);

        // Assert
        actualTokens.Should().Equal(["word1", "word2"]);
    }

    [Fact]
    public void SplitWords_ShouldSplitTextProperly_IfIgnoreNumbersIsTrue()
    {
        // Arrange
        var optionsValue = new PlagiarismCheckOptions
        {
            IgnoreNumbers = true,
            IgnoreCase = false
        };

        _options.Value.Returns(optionsValue);

        var textToSplit = "word1 word2";

        // Act

        var actualTokens = _sut.SplitWords(textToSplit);

        // Assert
        actualTokens.Should().Equal(["word", "word"]);
    }
    
    [Fact]
    public void SplitWords_ShouldSplitTextProperly_IfIgnoreCaseIsTrue()
    {
        // Arrange
        var optionsValue = new PlagiarismCheckOptions
        {
            IgnoreNumbers = false,
            IgnoreCase = true
        };

        _options.Value.Returns(optionsValue);

        var textToSplit = "WORD1 WORD2";

        // Act

        var actualTokens = _sut.SplitWords(textToSplit);

        // Assert
        actualTokens.Should().Equal(["word1", "word2"]);
    }
    
    [Fact]
    public void SplitWords_ShouldSplitTextProperly_IfIgnoreNumbersAndIgnoreCaseAreTrue()
    {
        // Arrange
        var optionsValue = new PlagiarismCheckOptions
        {
            IgnoreNumbers = true,
            IgnoreCase = true
        };

        _options.Value.Returns(optionsValue);

        var textToSplit = "WORD1 WORD2";

        // Act

        var actualTokens = _sut.SplitWords(textToSplit);

        // Assert
        actualTokens.Should().Equal(["word", "word"]);
    }
}