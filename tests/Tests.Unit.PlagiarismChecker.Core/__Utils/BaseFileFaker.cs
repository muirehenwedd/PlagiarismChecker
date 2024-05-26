using Bogus;
using PlagiarismChecker.Domain.Entities;

namespace Tests.Unit.PlagiarismChecker.Core.__Utils;

public sealed class BaseFileFaker
{
    public static Faker<BaseFile> Create()
    {
        return new Faker<BaseFile>()
            .CustomInstantiator(f => BaseFile.Create(
                f.System.FileName("txt"),
                Document.Create([], [], [], 0),
                BlobFileId.New()))
            .UseSeed(8);
    }
}