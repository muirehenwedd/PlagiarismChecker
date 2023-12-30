namespace PlagiarismChecker.Domain.Repository;

public interface IApplicationDbContextFactory
{
    IApplicationDbContext Create();
}