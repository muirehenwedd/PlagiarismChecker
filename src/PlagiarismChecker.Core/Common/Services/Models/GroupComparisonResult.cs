namespace PlagiarismChecker.Core.Common.Services.Models;

public sealed class GroupComparisonResult
{
    public required IReadOnlyCollection<DocumentComparisonResult> PairResults { get; init; }
}