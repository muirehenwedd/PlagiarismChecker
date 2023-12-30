using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Commands.CreateAssignment;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class CreateAssignmentEndpoint : IEndpoint<CreateAssignmentEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Post;
    public static string Route => "/assignments";

    public sealed record Parameters(
        [FromBody] Body Body,
        ClaimsPrincipal User,
        [FromServices] ISender Sender
    );

    public record Body(string AssignmentName);

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (body, user, sender) = parameters;
        var assignmentName = body.AssignmentName;

        var command = new CreateAssignmentCommand(assignmentName, user);
        var result = await sender.Send(command);

        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}