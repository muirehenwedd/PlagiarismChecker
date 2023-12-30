using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace PlagiarismCheck.Api.ExceptionHandling.ExceptionHandlers;

public sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not ValidationException validationException)
            return false;

        var errorsDictionary = validationException.Errors.ToDictionary(
            k => k.PropertyName,
            v => new[] {v.ErrorCode, v.ErrorMessage}
        );

        var result = TypedResults.ValidationProblem(errorsDictionary);

        await result.ExecuteAsync(httpContext);

        return true;
    }
}