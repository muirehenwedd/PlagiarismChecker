using PlagiarismChecker.Core.Common.Services;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Infrastructure.Services;

public sealed class DocumentInitializationService : IDocumentInitializationService
{
    private readonly IFileReaderService _fileReaderService;
    private readonly ITokenizerService _tokenizerService;
    private readonly ITokenHasherService _tokenHasherService;
    private readonly IHashSorterService _hashSorterService;

    public DocumentInitializationService(
        IFileReaderService fileReaderService,
        ITokenizerService tokenizerService,
        ITokenHasherService tokenHasherService,
        IHashSorterService hashSorterService
    )
    {
        _fileReaderService = fileReaderService;
        _tokenizerService = tokenizerService;
        _tokenHasherService = tokenHasherService;
        _hashSorterService = hashSorterService;
    }

    public Document Create(Stream fileStream, string contentType, string name)
    {
        var fileText = _fileReaderService.ReadFile(fileStream, contentType);

        var tokens = _tokenizerService.SplitWords(fileText);
        var documentOrderedHashes = _tokenHasherService.HashTokens(tokens);
        var (hashes, indexes) = _hashSorterService.GetNumericSortedHashes(documentOrderedHashes);

        var firstWordIndex = GetFirstWordIndex(tokens);

        var document = Document.Create(documentOrderedHashes, hashes, indexes, firstWordIndex);

        return document;
    }

    private static int GetFirstWordIndex(IEnumerable<string> tokens)
    {
        return tokens
            .Select((t, i) => new {Token = t, Index = i})
            .FirstOrDefault(pair => pair.Token.Length > 3)?.Index ?? 0;
    }
}