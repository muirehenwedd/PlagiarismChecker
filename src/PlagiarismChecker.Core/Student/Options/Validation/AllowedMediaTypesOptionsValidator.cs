using FluentValidation;

namespace PlagiarismChecker.Core.Student.Options.Validation;

public sealed class AllowedMediaTypesOptionsValidator : AbstractValidator<AllowedMediaTypesOptions>
{
    public AllowedMediaTypesOptionsValidator()
    {
        RuleFor(o => o).ForEach(o => o.NotEmpty());
    }
}