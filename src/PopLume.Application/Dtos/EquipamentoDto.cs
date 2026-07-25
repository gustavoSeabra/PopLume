namespace PopLume.Application.Dtos;

public class EquipamentoDto
{
    public Guid IdEquipamento { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Apelido { get; set; } = string.Empty;
    public DateOnly DataCompra { get; set; }
    public int Potencia { get; set; }
    public decimal ValorCompra { get; set; }
    public int ExpectativaVida { get; set; }
    public bool Excluido { get; set; }
    public DateTime? DataExclusao { get; set; }
}

public class CreateEquipamentoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Apelido { get; set; } = string.Empty;
    public DateOnly? DataCompra { get; set; }
    public int? Potencia { get; set; }
    public decimal? ValorCompra { get; set; }
    public int? ExpectativaVida { get; set; }
}

public class UpdateEquipamentoDto
{
    public Guid IdEquipamento { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Apelido { get; set; } = string.Empty;
    public DateOnly? DataCompra { get; set; }
    public int? Potencia { get; set; }
    public decimal? ValorCompra { get; set; }
    public int? ExpectativaVida { get; set; }
}
