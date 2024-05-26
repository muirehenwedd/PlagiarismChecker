using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Domain.Repository;

public interface IApplicationDbContext
{
    DbSet<BaseFile> BaseFiles { get; }
    DbSet<Assignment> StudentAssignments { get; }
    DbSet<User> Users { get; }
    DbSet<AssignmentFile> AssignmentFiles { get; }
    DbSet<Document> Documents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}