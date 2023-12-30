using System.Collections.Frozen;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.ExceptionHandling.Abstractions;

namespace PlagiarismCheck.Api.ExceptionHandling;

public sealed class ExceptionMapper : IExceptionMapper
{
    private readonly FrozenDictionary<Guid, ProblemDetailsRegistryEntry> _registry;

    public ExceptionMapper(IDictionary<Type, MapperSetupEntry> registryEntries)
    {
        _registry = registryEntries.ToFrozenDictionary(
            pair => pair.Key.GUID,
            pair => new ProblemDetailsRegistryEntry
            {
                Title = pair.Key.Name,
                Status = (int) pair.Value.StatusCode,
                Extensions = new Dictionary<string, object?>
                    {{Constants.CustomErrorCodePropertyName, pair.Value.CustomCode}}
            });
    }

    private record struct ProblemDetailsRegistryEntry(
        string Title,
        int Status,
        Dictionary<string, object?> Extensions
    );

    private static readonly ProblemDetails DefaultProblemDetails = new()
    {
        Title = "UnhandledException",
        Status = (int) HttpStatusCode.InternalServerError,
        Detail = "Unhandled exception occurred.",
        Extensions = new Dictionary<string, object?> {{Constants.CustomErrorCodePropertyName, 100}}
    };

    public ProblemDetails Map(Exception exception)
    {
        var exceptionTypeId = exception.GetType().GUID;
        var entryFound = _registry.TryGetValue(exceptionTypeId, out var entry);

        if (entryFound is false)
        {
            return DefaultProblemDetails;
        }

        return new ProblemDetails
        {
            Title = entry.Title,
            Status = entry.Status,
            Extensions = entry.Extensions,
            Detail = exception.Message
        };
    }
}