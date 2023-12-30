using Microsoft.AspNetCore.Mvc;

namespace PlagiarismCheck.Api.ExceptionHandling.Abstractions;

public interface IExceptionMapper
{
    ProblemDetails Map(Exception exception);
}