using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Infrastructure.Data.EntityConfiguration;

public sealed class DocumentEntityConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .HasConversion<DocumentId.EfCoreValueConverter>()
            .IsRequired();

        builder
            .PrimitiveCollection(e => e.DocumentSortedWordHashes)
            .IsRequired();

        builder
            .PrimitiveCollection(e => e.NumericOrderedWordHashes)
            .IsRequired();

        builder
            .PrimitiveCollection(e => e.NumericOrderedWordIndexes)
            .IsRequired();

        builder
            .Property(e => e.FirstWordIndex)
            .IsRequired();

        builder
            .Property(e => e.WordsCount)
            .IsRequired();
    }
}