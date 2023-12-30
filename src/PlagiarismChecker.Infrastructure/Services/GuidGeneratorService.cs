using PlagiarismChecker.Core.Common.Services;

namespace PlagiarismChecker.Infrastructure.Services;

public sealed class GuidGeneratorService : IGuidGeneratorService
{
    public Guid NewGuid() => Guid.NewGuid();
}