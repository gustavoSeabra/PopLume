namespace PopDesign.Domain.Entities;

public class TaxasMarketplace
{
    public Guid IdTaxa { get; set; }
    public Guid IdMarketplace { get; set; }
    public decimal ValorInicial { get; set; }
    public decimal ValorFinal { get; set; }
    public decimal Comissao { get; set; }
    public decimal TaxaFixa { get; set; }

    public virtual Marketplace Marketplace { get; set; } = null!;
}
