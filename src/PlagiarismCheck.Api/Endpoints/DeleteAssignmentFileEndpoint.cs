using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Commands.DeleteAssignmentFile;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class DeleteAssignmentFileEndpoint : IEndpoint<DeleteAssignmentFileEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Delete;
    public static string Route => "/assignments/{assignmentId:guid}/files/{assignmentFileId}";

    public sealed record Parameters(
        ClaimsPrincipal User,
        [FromServices] ISender Sender,
        [FromRoute] Guid AssignmentId,
        [FromRoute] Guid AssignmentFileId
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (user, sender, assignmentId, assignmentFileId) = parameters;

        var command = new DeleteAssignmentFileCommand(user, assignmentId, assignmentFileId);
        var result = await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}