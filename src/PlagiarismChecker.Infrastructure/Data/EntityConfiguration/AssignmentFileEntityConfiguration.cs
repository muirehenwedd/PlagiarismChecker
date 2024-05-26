using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Infrastructure.Data.EntityConfiguration;

public sealed class AssignmentFileEntityConfiguration : IEntityTypeConfiguration<AssignmentFile>
{
    public void Configure(EntityTypeBuilder<AssignmentFile> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .HasConversion<AssignmentFileId.EfCoreValueConverter>()
            .IsRequired();

        builder
            .Property(e => e.FileName)
            .IsRequired();

        builder
            .Property(e => e.BlobFileId)
            .HasConversion<BlobFileId.EfCoreValueConverter>()
            .IsRequired();

        builder
            .Property(e => e.AssignmentId)
            .HasConversion<AssignmentId.EfCoreValueConverter>()
            .IsRequired();

        builder
            .HasOne(e => e.Assignment)
            .WithMany(e => e.AssignmentFiles)
            .HasForeignKey(e => e.AssignmentId);

        builder
            .Property(e => e.DocumentId)
            .HasConversion<DocumentId.EfCoreValueConverter>()
            .IsRequired();

        builder
            .HasOne(e => e.Document)
            .WithOne()
            .HasForeignKey<AssignmentFile>(e => e.DocumentId);
    }
}