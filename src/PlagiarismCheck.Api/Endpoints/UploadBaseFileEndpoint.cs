using Mediator;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using PlagiarismCheck.Api.Authorization.Policies;
using PlagiarismCheck.Api.Endpoints.Abstractions;
using PlagiarismChecker.Core.Admin.Commands.UploadBaseFile;

namespace PlagiarismCheck.Api.Endpoints;

public sealed class UploadBaseFileEndpoint : IEndpoint<UploadBaseFileEndpoint.Parameters>
{
    public static HttpMethod Method => HttpMethod.Post;
    public static string Route => "/admin/files";

    public sealed record Parameters(
        [FromForm] IFormFileCollection FormFileCollection,
        [FromServices] ISender Sender
    );

    public static async Task<IResult> HandleAsync(Parameters parameters)
    {
        var (formFileCollection, sender) = parameters;
        var formFile = formFileCollection[0];

        await using var contentStream = formFile.OpenReadStream();

        var command = new UploadBaseFileCommand(contentStream, formFile.ContentType, formFile.FileName);

        var result = await sender.Send(command);

        return TypedResults.Json(result);
    }

    public static void Configure(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization(RequireAdminRolePolicy.Name);
        builder.Finally(RemoveAntiforgeryMetadata);
    }

    private static void RemoveAntiforgeryMetadata(EndpointBuilder obj)
    {
        var antiforgeryMetadata = obj.Metadata.First(o => o is IAntiforgeryMetadata);
        obj.Metadata.Remove(antiforgeryMetadata);
    }
}