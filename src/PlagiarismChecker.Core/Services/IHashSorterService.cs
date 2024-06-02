namespace PlagiarismChecker.Core.Services;

public interface IHashSorterService
{
    (long[] Hashes,int[] Indexes) GetNumericSortedHashes(IReadOnlyList<long> hashes);
}