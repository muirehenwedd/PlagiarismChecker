using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Infrastructure.Data.EntityConfiguration;

public sealed class BaseFileEntityConfiguration : IEntityTypeConfiguration<BaseFile>
{
    public void Configure(EntityTypeBuilder<BaseFile> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .HasConversion<BaseFileId.EfCoreValueConverter>()
            .IsRequired();

        builder
            .Property(e => e.Name)
            .IsRequired();

        builder
            .Property(e => e.DocumentId)
            .HasConversion<DocumentId.EfCoreValueConverter>()
            .IsRequired();

        builder
            .HasOne(e => e.Document)
            .WithOne()
            .HasForeignKey<BaseFile>(e => e.DocumentId);

        builder
            .Property(e => e.BlobFileId)
            .HasConversion<BlobFileId.EfCoreValueConverter>()
            .IsRequired();
    }
}