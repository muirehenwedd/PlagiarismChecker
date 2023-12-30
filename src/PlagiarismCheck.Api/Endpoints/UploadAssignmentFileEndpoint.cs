using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Student.Commands.UploadAssignmentFile;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class UploadAssignmentFileEndpoint : IEndpoint<UploadAssignmentFileEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Post;
    public static string Route => "/assignments/{assignmentId}/files";

    public sealed record Parameters(
        [FromForm] IFormFileCollection FormFileCollection,
        ClaimsPrincipal User,
        [FromRoute] Guid AssignmentId,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (formFileCollection, user, assignmentId, sender) = parameters;
        var formFile = formFileCollection[0];

        await using var contentStream = formFile.OpenReadStream();

        var command = new UploadAssignmentFileCommand(
            user,
            assignmentId,
            contentStream,
            formFile.ContentType,
            formFile.FileName
        );

        var result = await sender.Send(command);

        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireStudentRolePolicy.Name);
        builder.Finally(RemoveAntiforgeryMetadata);
    }

    private static void RemoveAntiforgeryMetadata(EndpointBuilder obj)
    {
        var antiforgeryMetadata = obj.Metadata.First(o => o is IAntiforgeryMetadata);
        obj.Metadata.Remove(antiforgeryMetadata);
    }
}