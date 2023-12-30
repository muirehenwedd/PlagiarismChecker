using FluentValidation;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Student.Options;

namespace PlagiarismChecker.Core.Student.Commands.UploadAssignmentFile;

public sealed class UploadAssignmentFileCommandValidator : AbstractValidator<UploadAssignmentFileCommand>
{
    private readonly IOptions<AllowedMediaTypesOptions> _options;

    public UploadAssignmentFileCommandValidator(IOptions<AllowedMediaTypesOptions> options)
    {
        _options = options;

        RuleFor(x => x.ContentType)
            .Must(BeAllowedContentType)
            .WithMessage("Media type '{PropertyValue}' is not allowed.");
    }

    private bool BeAllowedContentType(string contentType)
    {
        return _options.Value.Contains(contentType);
    }
}