using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Infrastructure.Data.Configuration;
using PlagiarismChecker.Infrastructure.Mappers.Configuration;
using PlagiarismChecker.Infrastructure.Options.Configuration;
using PlagiarismChecker.Infrastructure.Services.Configuration;
using PlagiarismChecker.Infrastructure.Storage.Configuration;

namespace PlagiarismChecker.Infrastructure.InfrastructureConfiguration;

public static class InfrastructureConfigurationExtensions
{
    public static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.RegisterOptions(configuration);
        services.RegisterData();
        services.RegisterStorage();
        services.RegisterServices();
        services.RegisterMappers();

        return services;
    }
}