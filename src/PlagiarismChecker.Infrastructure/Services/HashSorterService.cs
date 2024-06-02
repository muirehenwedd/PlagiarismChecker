using PlagiarismChecker.Core.Services;

namespace PlagiarismChecker.Infrastructure.Services;

public sealed class HashSorterService : IHashSorterService
{
    public (long[] Hashes, int[] Indexes) GetNumericSortedHashes(IReadOnlyList<long> hashes)
    {
        var pairs = hashes
            .Select((hash, i) => new {Index = i, Hash = hash})
            .OrderBy(pair => pair.Hash)
            .ToArray();

        var hashesArray = new long[pairs.Length];
        var indexesArray = new int[pairs.Length];

        for (var i = 0; i < pairs.Length; i++)
        {
            var pair = pairs[i];
            hashesArray[i] = pair.Hash;
            indexesArray[i] = pair.Index;
        }

        return (hashesArray, indexesArray);
    }
}