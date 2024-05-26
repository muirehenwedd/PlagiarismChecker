using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Infrastructure.Data.EntityConfiguration;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(e => e.Id)
            .HasConversion<UserId.EfCoreValueConverter>()
            .IsRequired();
    }
}