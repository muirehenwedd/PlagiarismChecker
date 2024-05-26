using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.ValueObjects;

namespace PlagiarismChecker.Core.Common.Services;

public interface IDocumentComparerService
{
    DocumentComparisonResult Compare(Document docL, Document docR, DocumentComparisonParameters comparisonParameters);
}