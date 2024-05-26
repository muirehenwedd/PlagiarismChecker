namespace PlagiarismChecker.Domain.ValueObjects;

public sealed record DocumentComparisonParameters(
    int MismatchTolerance,
    int MismatchPercentage,
    int PhraseLength,
    int WordThreshold
);