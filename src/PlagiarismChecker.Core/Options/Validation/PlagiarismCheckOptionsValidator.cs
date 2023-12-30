using FluentValidation;

namespace PlagiarismChecker.Core.Options.Validation;

public sealed class PlagiarismCheckOptionsValidator : AbstractValidator<PlagiarismCheckOptions>
{
    public PlagiarismCheckOptionsValidator()
    {
        RuleFor(o => o.MismatchTolerance)
            .GreaterThanOrEqualTo(0);

        RuleFor(o => o.MismatchPercentage)
            .GreaterThanOrEqualTo(0);

        RuleFor(o => o.PhraseLength)
            .GreaterThanOrEqualTo(0);

        RuleFor(o => o.WordThreshold)
            .GreaterThanOrEqualTo(0);
    }
}