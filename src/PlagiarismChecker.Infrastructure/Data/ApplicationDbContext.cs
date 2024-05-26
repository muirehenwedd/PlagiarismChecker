using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Domain.Repository;
using PlagiarismChecker.Infrastructure.Data.EntityConfiguration;

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AssignmentEntityConfiguration());
        builder.ApplyConfiguration(new AssignmentFileEntityConfiguration());
        builder.ApplyConfiguration(new BaseFileEntityConfiguration());
        builder.ApplyConfiguration(new DocumentEntityConfiguration());
        //builder.ApplyConfiguration(new UserEntityConfiguration());

        builder.Entity<User>().Property(e => e.Id).HasConversion<UserId.EfCoreValueConverter>();
        builder.Entity<IdentityRole<UserId>>().Property(e => e.Id).HasConversion<UserId.EfCoreValueConverter>();
        base.OnModelCreating(builder);
    }
}