using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class CheckForPlagiarismEndpoint : IEndpoint<CheckForPlagiarismEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Get;
    public static string Route => "/assignments/{assignmentId}/check";

    public sealed record Parameters(
        ClaimsPrincipal User,
        [FromRoute] Guid AssignmentId,
        [FromQuery] int? MismatchTolerance,
        [FromQuery] int? MismatchPercentage,
        [FromQuery] int? PhraseLength,
        [FromQuery] int? WordThreshold,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (user, assignmentId, mismatchTolerance, mismatchPercentage, phraseLength, wordThreshold, sender) =
            parameters;

        var query = new CheckForPlagiarismQuery(user,
            new AssignmentId(assignmentId),
            mismatchTolerance,
            mismatchPercentage,
            phraseLength,
            wordThreshold);

        var result = await sender.Send(query);

        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
    }
}