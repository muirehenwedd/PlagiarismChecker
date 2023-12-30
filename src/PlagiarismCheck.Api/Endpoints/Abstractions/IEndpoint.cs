using System.Diagnostics.CodeAnalysis;

namespace PlagiarismCheck.Api.Endpoints.Abstractions;

public interface IEndpoint<TParams>
{
    static abstract HttpMethod Method { get; }

    [StringSyntax("Route")]
    static abstract string Route { get; }

    static abstract Task<IResult> HandleAsync(TParams parameters);
    static abstract void Configure(RouteHandlerBuilder builder);
}