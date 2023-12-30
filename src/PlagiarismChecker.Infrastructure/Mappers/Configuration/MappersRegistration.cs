using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Core.Mappers;

namespace PlagiarismChecker.Infrastructure.Mappers.Configuration;

public static class MappersRegistration
{
    public static IServiceCollection RegisterMappers(this IServiceCollection services)
    {
        services.AddSingleton<IAssignmentMapper, AssignmentMapper>();
        services.AddSingleton<IAssignmentFileMapper, AssignmentFileMapper>();

        return services;
    }
}