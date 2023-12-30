using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using PlagiarismChecker.Core.Common.Services;

namespace PlagiarismChecker.Infrastructure.Services;

public sealed class FileReaderService : IFileReaderService
{
    public string ReadFile(Stream stream, string contentType)
    {
        stream.Seek(0, SeekOrigin.Begin);

        var fileTest = contentType switch
        {
            MediaTypeNames.Text.Plain => ReadTxtFile(stream),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ReadMicrosoftWordFile(stream),
            _ => throw new UnreachableException()
        };

        return fileTest;
    }

    private string ReadMicrosoftWordFile(Stream stream)
    {
        using var doc = new FileFormat.Words.Document(stream);
        var body = new FileFormat.Words.Body(doc);

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendJoin(' ', body.Paragraphs.Select(p => p.Text));

        return stringBuilder.ToString();
    }

    private string ReadTxtFile(Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }
}