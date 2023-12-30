using FluentValidation;

namespace PlagiarismChecker.Infrastructure.Options.Validation;

public sealed class AuthOptionsValidator : AbstractValidator<AuthOptions>
{
    public AuthOptionsValidator()
    {
        RuleFor(o => o.ApiKey).NotEmpty();
    }
}