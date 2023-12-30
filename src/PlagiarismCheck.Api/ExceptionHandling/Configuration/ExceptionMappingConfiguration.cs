using System.Net;
using PlagiarismCheck.Api.ExceptionHandling.Abstractions;
using PlagiarismChecker.Core.Admin.Exceptions;
using PlagiarismChecker.Core.Student.Exceptions;

namespace PlagiarismCheck.Api.ExceptionHandling.Configuration;

public static class ExceptionMappingConfiguration
{
    public static IServiceCollection ConfigureExceptionMapping(this IServiceCollection services)
    {
        var mapping = new Dictionary<Type, MapperSetupEntry>
        {
            [typeof(BadHttpRequestException)] = new(HttpStatusCode.BadRequest, 101),
            [typeof(AssignmentAccessDeniedException)] = new(HttpStatusCode.Forbidden, 201),
            [typeof(AssignmentAlreadyCreatedException)] = new(HttpStatusCode.Conflict, 202),
            [typeof(AssignmentNotFoundException)] = new(HttpStatusCode.NotFound, 203),
            [typeof(BaseFileNotFoundException)] = new(HttpStatusCode.NotFound, 204),
            [typeof(AssignmentFileNotFoundException)] = new(HttpStatusCode.NotFound, 205)
        };

        services.AddSingleton<IExceptionMapper>(new ExceptionMapper(mapping));

        return services;
    }
}