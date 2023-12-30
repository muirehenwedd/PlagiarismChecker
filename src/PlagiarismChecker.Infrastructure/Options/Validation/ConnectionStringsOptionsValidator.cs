using FluentValidation;

namespace PlagiarismChecker.Infrastructure.Options.Validation;

public sealed class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(o => o.Postgres).NotEmpty();
        RuleFor(o => o.BlobStorage).NotEmpty();
    }
}