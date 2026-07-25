namespace PopLume.Application.Dtos;

public class ProdutoDto
{
    public Guid IdProduto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoCusto { get; set; }
    public decimal QuantidadeFilamento { get; set; }
    public int TempoImpressao { get; set; }
    public List<ProdutoDto>? Componentes { get; set; }
}

public class CreateProdutoDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal? PrecoCusto { get; set; }
    public decimal? QuantidadeFilamento { get; set; }
    public int? TempoImpressao { get; set; }
    public List<ProdutoComponenteDto>? Componentes { get; set; }
}

public class UpdateProdutoDto
{
    public Guid IdProduto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal? PrecoCusto { get; set; }
    public decimal? QuantidadeFilamento { get; set; }
    public int? TempoImpressao { get; set; }
    public List<ProdutoComponenteDto>? Componentes { get; set; }
}

public class ProdutoComponenteDto
{
    public Guid IdProdutoFilho { get; set; }
    public int Quantidade { get; set; }
}
