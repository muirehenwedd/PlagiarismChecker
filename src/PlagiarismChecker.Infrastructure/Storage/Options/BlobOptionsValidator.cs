using FluentValidation;

namespace PlagiarismChecker.Infrastructure.Storage.Options;

public sealed class BlobOptionsValidator : AbstractValidator<BlobOptions>
{
    public BlobOptionsValidator()
    {
        RuleFor(o => o.ContainerName).NotEmpty();
    }
}