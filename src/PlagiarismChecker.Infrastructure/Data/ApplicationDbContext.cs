using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    )
        : base(options)
    {
    }

    public virtual DbSet<BaseFile> BaseFiles => Set<BaseFile>();
    public virtual DbSet<StudentAssignment> StudentAssignments => Set<StudentAssignment>();
    public virtual DbSet<AssignmentFile> AssignmentFiles => Set<AssignmentFile>();
    public virtual DbSet<Document> Documents => Set<Document>();
}