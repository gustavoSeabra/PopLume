using PopLume.Application.Dtos;
using PopLume.Domain.Entities;

namespace PopLume.Application.Mappers;

public static class ProdutoMapper
{
    public static ProdutoDto ToDto(this Produto produto)
    {
        if (produto == null) return null!;

        return new ProdutoDto
        {
            IdProduto = produto.IdProduto,
            Nome = produto.Nome,
            PrecoCusto = produto.PrecoCusto,
            QuantidadeFilamento = produto.QuantidadeFilamento,
            TempoImpressao = produto.TempoImpressao,
            Componentes = produto.ComposicoesPai?.Where(c => c.ProdutoFilho != null).Select(c => ToDto(c.ProdutoFilho!)).ToList()
        };
    }

    public static Produto ToEntity(this CreateProdutoDto dto)
    {
        if (dto == null) return null!;

        return new Produto
        {
            Nome = dto.Nome,
            PrecoCusto = dto.PrecoCusto.HasValue? dto.PrecoCusto.Value : 0,
            QuantidadeFilamento = dto.QuantidadeFilamento.HasValue? dto.QuantidadeFilamento.Value : 0,
            TempoImpressao = dto.TempoImpressao.HasValue? dto.TempoImpressao.Value : 0,
            ComposicoesPai = dto.Componentes?
                .Where(c => c.IdProdutoFilho != Guid.Empty)
                .Select(c => new ProdutoComposicao
                {
                    IdProdutoFilho = c.IdProdutoFilho,
                    Quantidade = c.Quantidade
                }).ToList() ?? new List<ProdutoComposicao>()
        };
    }
}
