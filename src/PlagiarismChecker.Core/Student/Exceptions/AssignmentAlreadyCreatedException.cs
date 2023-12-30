namespace PlagiarismChecker.Core.Student.Exceptions;

public sealed class AssignmentAlreadyCreatedException : Exception
{
    public AssignmentAlreadyCreatedException() : base("Assignment with this name was already created.")
    {
    }
}