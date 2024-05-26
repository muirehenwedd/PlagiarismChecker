using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Commands.DeleteAssignmentFile;
using PlagiarismChecker.Core.Student.Queries.GetAssignmentFile;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class GetAssignmentFileEndpoint : IEndpoint<GetAssignmentFileEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Get;
    public static string Route => "/assignments/{assignmentId:guid}/files/{assignmentFileId:guid}";

    public sealed record Parameters(
        ClaimsPrincipal User,
        [FromServices] ISender Sender,
        [FromRoute] Guid AssignmentId,
        [FromRoute] Guid AssignmentFileId
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (user, sender, assignmentId, assignmentFileId) = parameters;

        var command = new GetAssignmentFileQuery(user,
            new AssignmentId(assignmentId),
            new AssignmentFileId(assignmentFileId));

        var (fileContent, fileName, contentType) = await sender.Send(command);

        return TypedResults.File(fileContent, contentType, fileName);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}