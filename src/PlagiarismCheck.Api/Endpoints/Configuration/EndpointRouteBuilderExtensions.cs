using PlagiarismCheck.Api.Endpoints.Abstractions;

namespace PlagiarismCheck.Api.Endpoints.Configuration;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoint<TEndpoint, TParams>(this IEndpointRouteBuilder builder)
        where TEndpoint : IEndpoint<TParams>
    {
        var routeHandlerBuilder = builder.MapMethods(
            TEndpoint.Route,
            [TEndpoint.Method.Method],
            static ([AsParameters] TParams parameters) => TEndpoint.HandleAsync(parameters));

        TEndpoint.Configure(routeHandlerBuilder);

        return builder;
    }
}