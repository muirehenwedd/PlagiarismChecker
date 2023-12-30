using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Queries.GetAllAssignments;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class GetAllAssignmentsEndpoint : IEndpoint<GetAllAssignmentsEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Get;
    public static string Route => "/assignments";

    public sealed record Parameters(
        ClaimsPrincipal User,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (user, sender) = parameters;

        var query = new GetAllAssignmentsQuery(user);
        var result = await sender.Send(query);

        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}