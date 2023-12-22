namespace Domain.Entities;

public sealed class BaseFile
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
}