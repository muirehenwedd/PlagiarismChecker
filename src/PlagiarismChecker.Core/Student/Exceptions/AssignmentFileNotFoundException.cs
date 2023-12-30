namespace PlagiarismChecker.Core.Student.Exceptions;

public sealed class AssignmentFileNotFoundException : Exception
{
    public AssignmentFileNotFoundException()
        : base("Assignment file was not found.")
    {
    }
}