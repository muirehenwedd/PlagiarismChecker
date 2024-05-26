using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Commands.DeleteAssignment;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class DeleteAssignmentEndpoint : IEndpoint<DeleteAssignmentEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Delete;
    public static string Route => "/assignments/{assignmentId}";

    public sealed record Parameters(
        [FromRoute] Guid AssignmentId,
        ClaimsPrincipal User,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (assignmentId, user, sender) = parameters;

        var result = await sender.Send(new DeleteAssignmentCommand(user, new AssignmentId(assignmentId)));

        return TypedResults.NoContent();
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}