using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Common.Services;

public interface IDocumentInitializationService
{
    Document Create(Stream fileStream, string contentType, string name);
}