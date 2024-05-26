using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Queries.GetAssignment;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class GetAssignmentEndpoint : IEndpoint<GetAssignmentEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Get;
    public static string Route => "/assignments/{assignmentId}";

    public sealed record Parameters(
        [FromRoute] Guid AssignmentId,
        ClaimsPrincipal User,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (assignmentId, user, sender) = parameters;

        var query = new GetAssignmentQuery(user, new AssignmentId(assignmentId));
        var result = await sender.Send(query);

        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}