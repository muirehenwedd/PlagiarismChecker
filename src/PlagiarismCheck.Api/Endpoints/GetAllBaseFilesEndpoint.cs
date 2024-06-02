using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Admin.Queries.GetAllBaseFiles;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class GetAllBaseFilesEndpoint : IEndpoint<GetAllBaseFilesEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Get;
    public static string Route => "/admin/files";

    public sealed record Parameters(
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var result = await parameters.Sender.Send(GetAllBaseFilesQuery.Instance);
        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireAdminRolePolicy.Name);

        //builder.WithOpenApi(operation => { });
    }
}