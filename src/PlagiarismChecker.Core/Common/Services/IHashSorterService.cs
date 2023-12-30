namespace PlagiarismChecker.Core.Common.Services;

public interface IHashSorterService
{
    (long[] Hashes,int[] Indexes) GetNumericSortedHashes(IReadOnlyList<long> hashes);
}