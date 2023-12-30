using FluentValidation;

namespace PlagiarismChecker.Infrastructure.Options.Validation;

public sealed class JwtOptionsValidator : AbstractValidator<JwtOptions>
{
    public JwtOptionsValidator()
    {
        RuleFor(o => o.Secret)
            .NotEmpty();

        RuleFor(o => o.Issuer)
            .NotEmpty();

        RuleFor(o => o.Audience)
            .NotEmpty();
    }
}