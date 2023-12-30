using System.Runtime.InteropServices;
using System.Security.Cryptography;
using PlagiarismChecker.Core.Common.Services;

namespace PlagiarismChecker.Infrastructure.Services;

public sealed class TokenHasherService : ITokenHasherService
{
    public long[] HashTokens(IReadOnlyList<string> tokens)
    {
        return tokens
            .Select(t => HashString(t))
            .ToArray();
    }

    private long HashString(in string input)
    {
        var inputAsBytesSpan = MemoryMarshal.AsBytes(input.AsSpan());

        var hashData = SHA256.HashData(inputAsBytesSpan);
        var longHash = BitConverter.ToInt64(hashData[..8]);

        return longHash;
    }
}