namespace PlagiarismChecker.Core.Student.Exceptions;

public sealed class AssignmentAccessDeniedException : Exception
{
    public AssignmentAccessDeniedException() : base("User has no rights to access this assignment.")
    {
    }
}