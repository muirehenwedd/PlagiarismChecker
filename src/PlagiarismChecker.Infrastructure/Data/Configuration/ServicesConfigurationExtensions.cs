using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Domain.Repository;
using PlagiarismChecker.Infrastructure.Options;

namespace PlagiarismChecker.Infrastructure.Data.Configuration;

public static class DataConfigurationExtensions
{
    internal static IServiceCollection RegisterData(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(
            (provider, options) =>
            {
                options.UseNpgsql(provider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.Postgres);
            }, ServiceLifetime.Transient);

        serviceCollection.AddTransient<IApplicationDbContext>(static p => p.GetRequiredService<ApplicationDbContext>());
        serviceCollection.AddScoped<IApplicationDbContextFactory, ApplicationDbContextFactory>();

        return serviceCollection;
    }
}