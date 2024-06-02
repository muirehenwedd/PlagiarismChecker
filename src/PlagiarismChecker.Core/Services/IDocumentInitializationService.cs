using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Services;

public interface IDocumentInitializationService
{
    Document Create(Stream fileStream, string contentType, string name);
}