using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopLume.Domain.Entities;

namespace PopLume.Infrastructure.DataProvider.EntityConfigurations;

public class TaxasMarketplaceEntityConfiguration : IEntityTypeConfiguration<TaxasMarketplace>
{
    public void Configure(EntityTypeBuilder<TaxasMarketplace> builder)
    {
        builder.ToTable("TaxasMarketplace");
        builder.HasKey(e => e.IdTaxa);

        builder.Property(e => e.ValorInicial).HasPrecision(10, 2);
        builder.Property(e => e.ValorFinal).HasPrecision(10, 2);
        builder.Property(e => e.Comissao).HasPrecision(5, 2).IsRequired();
        builder.Property(e => e.TaxaFixa).HasPrecision(10, 2);

        builder.HasOne(e => e.Marketplace)
            .WithMany(e => e.TaxasMarketplace)
            .HasForeignKey(e => e.IdMarketplace)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
