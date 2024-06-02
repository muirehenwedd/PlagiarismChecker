namespace PlagiarismChecker.Core.Services;

public interface IFileReaderService
{
    string ReadFile(Stream stream, string contentType);
}