namespace PlagiarismChecker.Core.Common.Services;

public interface ITokenHasherService
{
    long[] HashTokens(IReadOnlyList<string> tokens);
}