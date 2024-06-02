namespace PlagiarismChecker.Core.Services;

public interface ITokenHasherService
{
    long[] HashTokens(IReadOnlyList<string> tokens);
}