using Microsoft.AspNetCore.Diagnostics;
using PlagiarismCheck.Api.ExceptionHandling.Abstractions;

namespace PlagiarismCheck.Api.ExceptionHandling.ExceptionHandlers;

public sealed class CommonExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var exceptionMapper = httpContext.RequestServices.GetRequiredService<IExceptionMapper>();
        var problemDetail = exceptionMapper.Map(exception);

        var result = TypedResults.Problem(problemDetail);

        await result.ExecuteAsync(httpContext);

        return true;
    }
}