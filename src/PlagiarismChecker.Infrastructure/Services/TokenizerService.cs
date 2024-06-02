using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Options;
using PlagiarismChecker.Core.Services;
using PlagiarismChecker.Infrastructure.Options;

namespace PlagiarismChecker.Infrastructure.Services;

public sealed class TokenizerService : ITokenizerService
{
    private readonly IOptions<PlagiarismCheckOptions> _options;

    public TokenizerService(IOptions<PlagiarismCheckOptions> options)
    {
        _options = options;
    }

    private static readonly char[] NumberChars =
        Enumerable.Range(0, 10).Select(i => char.Parse(i.ToString())).ToArray();

    public IReadOnlyList<string> SplitWords(string text)
    {
        var punctuation = text
            .Where(char.IsPunctuation)
            .Distinct()
            .ToArray();

        var split = text
            .Split()
            .Select(x => x.Trim(punctuation))
            .ToArray();

        if (_options.Value.IgnoreNumbers)
        {
            split = split.Select(s => s.Trim(NumberChars)).ToArray();
        }

        if (_options.Value.IgnoreCase)
        {
            split = split.Select(s => s.ToLowerInvariant()).ToArray();
        }

        var emptyRemoved = split.Where(t => string.IsNullOrWhiteSpace(t) is false).ToArray();

        return emptyRemoved;
    }
}