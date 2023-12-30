using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Admin.Commands.DeleteTrustedFileByName;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class DeleteBaseFileByNameEndpoint : IEndpoint<DeleteBaseFileByNameEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Delete;
    public static string Route => "/admin/files/{name}";

    public sealed record Parameters(
        [FromRoute] string Name,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (name, sender) = parameters;

        var command = new DeleteBaseFileByNameCommand(name);
        var result = await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireAdminRolePolicy.Name);
    }
}