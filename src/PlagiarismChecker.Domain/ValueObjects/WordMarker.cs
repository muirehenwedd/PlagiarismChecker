namespace PlagiarismChecker.Domain.ValueObjects;

public enum WordMarker : byte
{
    WordUnmatched = 0,
    WordPerfect = 1,
    WordFlaw = 2
}