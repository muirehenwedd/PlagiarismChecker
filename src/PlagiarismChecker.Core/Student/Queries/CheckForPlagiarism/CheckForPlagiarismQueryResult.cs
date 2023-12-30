namespace PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;

public sealed record CheckForPlagiarismQueryResult(
    bool PlagiarismFound,
    IEnumerable<CheckForPlagiarismQueryResult.Match> Matches
)
{
    public sealed record Match(
        string DocumentLeftName,
        string DocumentRightName,
        int MatchingWordPerfect,
        int MatchingWordTotalL,
        int MatchingWordTotalR,
        decimal MatchingPercentL,
        decimal MatchingPercentR
    );
}