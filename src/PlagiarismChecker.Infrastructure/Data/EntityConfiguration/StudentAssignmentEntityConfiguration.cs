using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Infrastructure.Data.EntityConfiguration;

public sealed class StudentAssignmentEntityConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.OwnerId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .IsRequired();

        builder
            .Property(e => e.CreationTimestamp)
            .IsRequired();

        builder
            .HasMany(e => e.AssignmentFiles)
            .WithOne(e => e.Assignment)
            .HasForeignKey(e => e.AssignmentId);

        builder
            .HasIndex(e => new {e.Name, e.OwnerId})
            .IsUnique();
    }
}