namespace PopDesign.Application.Dtos;

public class TaxasMarketplaceDto
{
    public Guid IdTaxa { get; set; }
    public Guid IdMarketplace { get; set; }
    public decimal ValorInicial { get; set; }
    public decimal ValorFinal { get; set; }
    public int Comissao { get; set; }
    public decimal TaxaFixa { get; set; }
    public string MarketplaceNome { get; set; } = string.Empty;
}
