using PlagiarismChecker.Core.Common.Services.Models;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Common.Services;

public interface IDocumentComparerService
{
    DocumentComparisonResult Compare(Document docL, Document docR, DocumentComparisonParameters comparisonParameters);
}