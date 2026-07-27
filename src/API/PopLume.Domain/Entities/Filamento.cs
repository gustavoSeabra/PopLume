using PopLume.Domain.Enums;

namespace PopLume.Domain.Entities;

public class Filamento
{
    public Guid IdFilamento { get; set; }
    public string Cor { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public decimal Peso { get; set; }
    public TipoFilamento Tipo { get; set; }
    public DateOnly DataCompra { get; set; }
}
