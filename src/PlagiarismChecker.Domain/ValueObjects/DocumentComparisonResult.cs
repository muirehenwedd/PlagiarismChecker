using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Domain.ValueObjects;

public sealed class DocumentComparisonResult
{
    public required Document DocumentL { get; init; }
    public required Document DocumentR { get; init; }
    public required int PerfectMatch { get; init; }
    public required decimal PerfectMatchPercentLeft { get; init; }
    public required decimal PerfectMatchPercentRight { get; init; }
    public required int OverallMatchCountLeft { get; init; }
    public required int OverallMatchCountRight { get; init; }
    public required decimal OverallMatchPercentLeft { get; init; }
    public required decimal OverallMatchPercentRight { get; init; }
    public required IReadOnlyList<WordMarker> WordMarkersLeft { get; init; }
    public required IReadOnlyList<WordMarker> WordMarkersRight { get; init; }
}