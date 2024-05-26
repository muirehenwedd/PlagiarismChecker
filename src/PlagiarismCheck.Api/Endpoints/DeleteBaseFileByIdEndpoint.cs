using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Admin.Commands.DeleteTrustedFileById;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class DeleteBaseFileByIdEndpoint : IEndpoint<DeleteBaseFileByIdEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Delete;
    public static string Route => "/admin/files/{fileId:guid}";

    public sealed record Parameters(
        [FromRoute] Guid FileId,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (fileId, sender) = parameters;

        var command = new DeleteBaseFileByIdCommand(new BaseFileId(fileId));
        var result = await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireAdminRolePolicy.Name);
    }
}