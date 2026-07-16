namespace PopDesign.Domain.Entities;

public class Marketplace
{
    public Guid IdMarketplace { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string LinkLoja { get; set; } = string.Empty;
    public bool Excluido { get; private set; }
    public DateTime? DataExclusao { get; private set; }

    public virtual ICollection<TaxasMarketplace> TaxasMarketplace { get; set; } = new List<TaxasMarketplace>();

    public void Excluir()
    {
        Excluido = true;
        DataExclusao = DateTime.UtcNow;
    }

    public void Restaurar()
    {
        Excluido = false;
        DataExclusao = null;
    }
}
