using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class CheckForPlagiarismEndpoint : IEndpoint<CheckForPlagiarismEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Get;
    public static string Route => "/assignments/{assignmentId}/check";

    public sealed record Parameters(
        [FromRoute] Guid AssignmentId,
        ClaimsPrincipal User,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (assignmentId, user, sender) = parameters;

        var result = await sender.Send(new CheckForPlagiarismQuery(user, assignmentId));

        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}