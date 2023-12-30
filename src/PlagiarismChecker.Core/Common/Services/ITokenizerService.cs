namespace PlagiarismChecker.Core.Common.Services;

public interface ITokenizerService
{
    IReadOnlyList<string> SplitWords(string text);
}