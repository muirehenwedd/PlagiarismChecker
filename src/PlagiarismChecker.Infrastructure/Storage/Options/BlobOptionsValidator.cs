using FluentValidation;

namespace PlagiarismChecker.Infrastructure.Storage.Options;

public sealed class BlobOptionsValidator : AbstractValidator<BlobOptions>
{
    public BlobOptionsValidator()
    {
        RuleFor(o => o.ContainerName).NotEmpty();

        RuleFor(o => o.ServiceUri)
            .NotNull()
            .NotEmpty()
            .Must(o => Uri.IsWellFormedUriString(o, UriKind.Absolute)).WithMessage("Must be well-formed uri string.");
    }
}