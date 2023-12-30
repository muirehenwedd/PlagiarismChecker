using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Common.Services.Models;

public sealed class DocumentComparisonResult
{
    public required Document DocumentL { get; init; }
    public required Document DocumentR { get; init; }
    public required int MatchingWordsPerfect { get; init; }
    public required int MatchingWordsTotalL { get; init; }
    public required int MatchingWordsTotalR { get; init; }
    public required decimal MatchingPercentL { get; init; }
    public required decimal MatchingPercentR { get; init; }
    public required IReadOnlyList<WordMarker> WordMarkersL { get; init; }
    public required IReadOnlyList<WordMarker> WordMarkersR { get; init; }
}