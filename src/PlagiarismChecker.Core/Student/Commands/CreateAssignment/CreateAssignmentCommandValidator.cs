using FluentValidation;

namespace PlagiarismChecker.Core.Student.Commands.CreateAssignment;

public sealed class CreateAssignmentCommandValidator : AbstractValidator<CreateAssignmentCommand>
{
    public CreateAssignmentCommandValidator()
    {
        RuleFor(x => x.Name).Length(4, 30);
    }
}