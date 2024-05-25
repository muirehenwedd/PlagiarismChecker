namespace PlagiarismChecker.Core.Common.Services.Models;

public sealed record DocumentComparisonParameters(
    int MismatchTolerance,
    int MismatchPercentage,
    int PhraseLength,
    int WordThreshold
);