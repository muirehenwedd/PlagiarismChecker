namespace PlagiarismChecker.Core.Services;

public interface ITokenizerService
{
    IReadOnlyList<string> SplitWords(string text);
}