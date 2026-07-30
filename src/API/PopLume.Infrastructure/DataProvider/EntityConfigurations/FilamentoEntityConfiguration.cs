using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopLume.Domain.Entities;
using PopLume.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PopLume.Infrastructure.DataProvider.EntityConfigurations;

public class FilamentoEntityConfiguration : IEntityTypeConfiguration<Filamento>
{
    public void Configure(EntityTypeBuilder<Filamento> builder)
    {
        builder.ToTable("Filamento", table =>
            table.HasCheckConstraint(
                "CK_Filamento_Tipo",
                "\"Tipo\" IN ('ABS', 'PETG', 'PLA', 'TPU')"));

        builder.HasKey(f => f.IdFilamento);
        builder.Property(f => f.Cor).IsRequired().HasMaxLength(50);
        builder.Property(f => f.Valor).IsRequired().HasPrecision(10, 2);
        builder.Property(f => f.Peso).IsRequired().HasPrecision(10, 2);
        builder.Property(f => f.Tipo)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<TipoFilamento>())
            .HasMaxLength(10);
        builder.Property(f => f.DataCompra).HasColumnType("date").IsRequired();
    }
}
