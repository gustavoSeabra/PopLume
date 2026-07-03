namespace PopDesign.Application.Dtos;

public class MarketplaceDto
{
    public Guid IdMarketplace { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string LinkLoja { get; set; } = string.Empty;
    public bool Excluido { get; set; }
    public DateTime? DataExclusao { get; set; }
    public List<TaxasMarketplaceDto>? TaxasMarketplace { get; set; }
}

public class CreateMarketplaceDto
{
    public string Nome { get; set; } = string.Empty;
    public string LinkLoja { get; set; } = string.Empty;
    public List<CreateMarketplaceTaxaDto>? TaxasMarketplace { get; set; }
}

public class UpdateMarketplaceDto
{
    public Guid IdMarketplace { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string LinkLoja { get; set; } = string.Empty;
    public List<UpdateMarketplaceTaxaDto>? TaxasMarketplace { get; set; }
}

public class CreateMarketplaceTaxaDto
{
    public decimal? ValorInicial { get; set; }
    public decimal? ValorFinal { get; set; }
    public int? Comissao { get; set; }
    public decimal? TaxaFixa { get; set; }
}

public class UpdateMarketplaceTaxaDto
{
    public Guid? IdTaxa { get; set; }
    public decimal? ValorInicial { get; set; }
    public decimal? ValorFinal { get; set; }
    public int? Comissao { get; set; }
    public decimal? TaxaFixa { get; set; }
}
