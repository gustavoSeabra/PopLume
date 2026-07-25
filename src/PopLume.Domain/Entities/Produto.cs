namespace PopLume.Domain.Entities;

public class Produto
{
    public Guid IdProduto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoCusto { get; set; }
    public decimal QuantidadeFilamento { get; set; }
    public int TempoImpressao { get; set; }

    // Propriedades de Navegação
    // Componentes que formam este produto
    public virtual ICollection<ProdutoComposicao> ComposicoesPai { get; set; } = new List<ProdutoComposicao>();
    // Onde este produto é usado como componente
    public virtual ICollection<ProdutoComposicao> ComposicoesFilho { get; set; } = new List<ProdutoComposicao>();
}
