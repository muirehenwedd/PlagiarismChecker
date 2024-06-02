using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Core.Admin.Commands.UploadBaseFile;
using PlagiarismChecker.Core.Behaviours;
using PlagiarismChecker.Core.Student.Commands.CreateAssignment;
using PlagiarismChecker.Core.Student.Commands.UploadAssignmentFile;

namespace PlagiarismChecker.Core.CoreConfiguration;

public static class CoreConfigurationExtensions
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
    {
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddScoped<IValidator<CreateAssignmentCommand>, CreateAssignmentCommandValidator>();
        services.AddScoped<IValidator<UploadAssignmentFileCommand>, UploadAssignmentFileCommandValidator>();
        services.AddScoped<IValidator<UploadBaseFileCommand>, UploadBaseFileCommandValidator>();
        return services;
    }
}