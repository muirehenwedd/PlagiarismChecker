using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Infrastructure.Data;

public sealed class ApplicationDbContextFactory : IApplicationDbContextFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationDbContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IApplicationDbContext Create()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
}