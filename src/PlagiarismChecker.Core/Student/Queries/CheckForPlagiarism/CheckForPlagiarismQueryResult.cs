namespace PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;

public sealed record CheckForPlagiarismQueryResult(
    bool PlagiarismFound,
    IEnumerable<CheckForPlagiarismQueryResult.Match> Matches
)
{
    public sealed record Match(
        string DocumentNameLeft,
        string DocumentNameRight,
        int PerfectMatch,
        decimal PerfectMatchPercentLeft,
        decimal PerfectMatchPercentRight,
        int OverallMatchCountLeft,
        int OverallMatchCountRight,
        decimal OverallMatchPercentLeft,
        decimal OverallMatchPercentRight
    );
}