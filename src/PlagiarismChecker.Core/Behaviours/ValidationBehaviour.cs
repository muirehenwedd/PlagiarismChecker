using FluentValidation;
using Mediator;

namespace PlagiarismChecker.Core.Behaviours;

public sealed class ValidationBehaviour<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    private readonly IEnumerable<IValidator<TMessage>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TMessage>> validators)
    {
        _validators = validators;
    }

    public async ValueTask<TResponse> Handle(
        TMessage message,
        CancellationToken cancellationToken,
        MessageHandlerDelegate<TMessage, TResponse> next
    )
    {
        // Validate
        var validationContext = new ValidationContext<TMessage>(message);

        var validationFailures = _validators
            .Select(validator => validator.Validate(validationContext))
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        if (validationFailures.Length > 0)
        {
            throw new ValidationException(validationFailures);
        }

        var result = await next(message, cancellationToken);
        return result;
    }
}