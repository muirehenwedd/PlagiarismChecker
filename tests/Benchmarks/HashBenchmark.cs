using System.Runtime.InteropServices;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser(true)]
public class HashBenchmark
{
    public const string InputString = "la li lu le lo";

    [Benchmark]
    public long Md5()
    {
        var inputAsBytesSpan = MemoryMarshal.AsBytes(InputString.AsSpan());

        var hashData = MD5.HashData(inputAsBytesSpan);
        var longHash = BitConverter.ToInt64(hashData.AsSpan()[..8]);

        return longHash;
    }

    [Benchmark]
    public long Md5_Span()
    {
        var inputAsBytesSpan = MemoryMarshal.AsBytes(InputString.AsSpan());

        Span<byte> hashData = stackalloc byte[8];
        MD5.TryHashData(inputAsBytesSpan, hashData, out _);
        var longHash = BitConverter.ToInt64(hashData[..8]);

        return longHash;
    }

    [Benchmark]
    public long Sha256()
    {
        var inputAsBytesSpan = MemoryMarshal.AsBytes(InputString.AsSpan());

        var hashData = SHA256.HashData(inputAsBytesSpan);
        var longHash = BitConverter.ToInt64(hashData.AsSpan()[..8]);

        return longHash;
    }

    [Benchmark]
    public long Sha256_Span()
    {
        var inputAsBytesSpan = MemoryMarshal.AsBytes(InputString.AsSpan());

        Span<byte> hashData = stackalloc byte[8];
        SHA256.TryHashData(inputAsBytesSpan, hashData, out _);
        var longHash = BitConverter.ToInt64(hashData[..8]);

        return longHash;
    }
}