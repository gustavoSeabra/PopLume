namespace PopLume.Domain.Entities;

public class Equipamento
{
    public Guid IdEquipamento { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Apelido { get; set; } = string.Empty;
    public DateOnly DataCompra { get; set; }
    public int Potencia { get; set; }
    public decimal ValorCompra { get; set; }
    public int ExpectativaVida { get; set; }
    public decimal ValorHora { get; private set; }
    public bool Excluido { get; private set; }
    public DateTime? DataExclusao { get; private set; }

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
