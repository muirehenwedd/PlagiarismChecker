namespace PlagiarismChecker.Domain.Entities;

public sealed class Document
{
    public DocumentId Id { get; init; }
    public required long[] DocumentSortedWordHashes { get; init; }
    public required long[] NumericOrderedWordHashes { get; init; }
    public required int[] NumericOrderedWordIndexes { get; init; }
    public required int FirstFileIndex { get; init; }

    public required int WordsCount { get; init; }
}