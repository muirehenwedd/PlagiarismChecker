namespace PlagiarismChecker.Core.Student.Exceptions;

public sealed class AssignmentNotFoundException : Exception
{
    public AssignmentNotFoundException()
        : base("Assignment was not found.")
    {
    }
}