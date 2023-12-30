namespace PlagiarismCheck.Api.Endpoints.Configuration;

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapEndpoint<UploadBaseFileEndpoint, UploadBaseFileEndpoint.Parameters>();
        builder.MapEndpoint<DeleteBaseFileByNameEndpoint, DeleteBaseFileByNameEndpoint.Parameters>();
        builder.MapEndpoint<DeleteBaseFileByIdEndpoint, DeleteBaseFileByIdEndpoint.Parameters>();
        builder.MapEndpoint<GetAllBaseFilesEndpoint, GetAllBaseFilesEndpoint.Parameters>();

        return builder;
    }

    public static IEndpointRouteBuilder MapStudentEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapEndpoint<CreateAssignmentEndpoint, CreateAssignmentEndpoint.Parameters>();
        builder.MapEndpoint<UploadAssignmentFileEndpoint, UploadAssignmentFileEndpoint.Parameters>();
        builder.MapEndpoint<GetAssignmentFileEndpoint, GetAssignmentFileEndpoint.Parameters>();
        builder.MapEndpoint<DeleteAssignmentFileEndpoint, DeleteAssignmentFileEndpoint.Parameters>();
        builder.MapEndpoint<GetAssignmentEndpoint, GetAssignmentEndpoint.Parameters>();
        builder.MapEndpoint<GetAllAssignmentsEndpoint, GetAllAssignmentsEndpoint.Parameters>();
        builder.MapEndpoint<DeleteAssignmentEndpoint, DeleteAssignmentEndpoint.Parameters>();
        builder.MapEndpoint<CheckForPlagiarismEndpoint, CheckForPlagiarismEndpoint.Parameters>();

        return builder;
    }
}