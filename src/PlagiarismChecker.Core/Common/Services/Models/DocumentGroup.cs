using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Common.Services.Models;

public sealed class DocumentGroup
{
    public required int GroupIdentifier { get; init; }
    public required IReadOnlyCollection<Document> Documents { get; init; }
}