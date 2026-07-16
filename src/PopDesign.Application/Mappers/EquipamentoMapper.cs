using PopDesign.Application.Dtos;
using PopDesign.Domain.Entities;

namespace PopDesign.Application.Mappers;

public static class EquipamentoMapper
{
    public static EquipamentoDto ToDto(this Equipamento equipamento)
    {
        if (equipamento == null) return null!;

        return new EquipamentoDto
        {
            IdEquipamento = equipamento.IdEquipamento,
            Nome = equipamento.Nome,
            Apelido = equipamento.Apelido,
            DataCompra = equipamento.DataCompra,
            Potencia = equipamento.Potencia,
            ValorCompra = equipamento.ValorCompra,
            ExpectativaVida = equipamento.ExpectativaVida,
            Excluido = equipamento.Excluido,
            DataExclusao = equipamento.DataExclusao
        };
    }

    public static Equipamento ToEntity(this CreateEquipamentoDto dto)
    {
        if (dto == null) return null!;

        return new Equipamento
        {
            Nome = dto.Nome,
            Apelido = dto.Apelido,
            DataCompra = dto.DataCompra!.Value,
            Potencia = dto.Potencia ?? 0,
            ValorCompra = dto.ValorCompra ?? 0m,
            ExpectativaVida = dto.ExpectativaVida ?? 0
        };
    }
}
