namespace PlagiarismChecker.Core.Common.Services;

public interface IFileReaderService
{
    string ReadFile(Stream stream, string contentType);
}