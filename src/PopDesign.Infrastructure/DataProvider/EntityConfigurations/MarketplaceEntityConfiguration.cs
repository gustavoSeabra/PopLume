using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopDesign.Domain.Entities;

namespace PopDesign.Infrastructure.DataProvider.EntityConfigurations;

public class MarketplaceEntityConfiguration : IEntityTypeConfiguration<Marketplace>
{
    public void Configure(EntityTypeBuilder<Marketplace> builder)
    {
        builder.ToTable("Marketplace");
        builder.HasKey(e => e.IdMarketplace);
        builder.HasQueryFilter(e => !e.Excluido);

        builder.Property(e => e.Nome).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LinkLoja).HasMaxLength(100);
        builder.Property(e => e.Excluido).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.DataExclusao);
    }
}
