using FluentAssertions;
using PlagiarismChecker.Infrastructure.Services;

namespace Tests.Unit.PlagiarismChecker.Infrastructure.Services;

public sealed class HashSorterServiceTests
{
    private HashSorterService _sut;

    public HashSorterServiceTests()
    {
        _sut = new HashSorterService();
    }

    [Fact]
    public void GetNumericSortedHashes_ShouldReturnNumericSortedHashes()
    {
        // Arrange
        long[] hashes = [3, 2, 1];

        // Act
        var actualOutput = _sut.GetNumericSortedHashes(hashes);

        // Assert
        var actualSortedHashes = actualOutput.Hashes;
        actualSortedHashes.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void GetNumericSortedHashes_ShouldReturnActualWordIndex()
    {
        // Arrange
        var hashAndIndexMap = new Dictionary<long, int>
        {
            [3] = 0,
            [2] = 1,
            [1] = 2
        };

        // Act
        var actualOutput = _sut.GetNumericSortedHashes(hashAndIndexMap.Keys.ToArray());

        // Assert
        var (hashes, indexes) = actualOutput;
        
        hashes.Zip(indexes, (_1, _2) => new {Hash = _1, WordIndex = _2})
            .Should().AllSatisfy(pair =>
            hashAndIndexMap[pair.Hash].Should().Be(pair.WordIndex)
        );
    }
}