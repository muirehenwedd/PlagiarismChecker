namespace PlagiarismChecker.Domain.Entities;

public sealed class BaseFile
{
    private BaseFile()
    {
    }

    public static BaseFile Create(string name, Document document, BlobFileId blobFileId)
    {
        return new BaseFile
        {
            Id = BaseFileId.New(),
            Name = name,
            Document = document,
            DocumentId = document.Id,
            BlobFileId = blobFileId
        };
    }

    public BaseFileId Id { get; private set; }
    public string Name { get; private set; }
    public DocumentId DocumentId { get; private set; }
    public Document Document { get; private set; }
    public BlobFileId BlobFileId { get; private set; }
}