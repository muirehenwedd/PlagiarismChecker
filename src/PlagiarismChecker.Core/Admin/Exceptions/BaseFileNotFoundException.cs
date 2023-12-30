namespace PlagiarismChecker.Core.Admin.Exceptions;

public sealed class BaseFileNotFoundException : Exception
{
    public BaseFileNotFoundException()
        : base("File was not found.")
    {
    }
}