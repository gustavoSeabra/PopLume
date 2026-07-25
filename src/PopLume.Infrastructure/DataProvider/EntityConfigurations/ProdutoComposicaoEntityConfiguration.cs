using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopLume.Domain.Entities;

namespace PopLume.Infrastructure.DataProvider.EntityConfigurations;

public class ProdutoComposicaoEntityConfiguration : IEntityTypeConfiguration<ProdutoComposicao>
{
    public void Configure(EntityTypeBuilder<ProdutoComposicao> builder)
    {
        builder.ToTable("Produto_Composicao");
        builder.HasKey(e => e.IdProdutoComposicao);

        builder.Property(e => e.Quantidade).HasDefaultValue(1);

        // Configuração do relacionamento autorreferenciado
        builder.HasOne(pc => pc.ProdutoPai)
            .WithMany(p => p.ComposicoesPai)
            .HasForeignKey(pc => pc.IdProdutoPai)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(pc => pc.ProdutoFilho)
            .WithMany(p => p.ComposicoesFilho)
            .HasForeignKey(pc => pc.IdProdutoFilho)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(pc => new { pc.IdProdutoPai, pc.IdProdutoFilho }).IsUnique();
    }
}