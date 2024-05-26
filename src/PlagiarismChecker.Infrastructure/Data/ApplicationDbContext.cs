using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;

namespace PlagiarismChecker.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<UserId>, UserId>, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    )
        : base(options)
    {
    }

    public virtual DbSet<BaseFile> BaseFiles => Set<BaseFile>();
    public virtual DbSet<Assignment> StudentAssignments => Set<Assignment>();
    public virtual DbSet<AssignmentFile> AssignmentFiles => Set<AssignmentFile>();
    public virtual DbSet<Document> Documents => Set<Document>();
}