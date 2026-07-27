using PopLume.Application.Dtos;
using PopLume.Domain.Entities;

namespace PopLume.Application.Mappers;

public static class FilamentoMapper
{
    public static FilamentoDto ToDto(this Filamento filamento) =>
        new()
        {
            IdFilamento = filamento.IdFilamento,
            Cor = filamento.Cor,
            Valor = filamento.Valor,
            Peso = filamento.Peso,
            Tipo = filamento.Tipo,
            DataCompra = filamento.DataCompra
        };

    public static Filamento ToEntity(this CreateFilamentoDto dto) =>
        new()
        {
            Cor = dto.Cor,
            Valor = dto.Valor!.Value,
            Peso = dto.Peso!.Value,
            Tipo = dto.Tipo!.Value,
            DataCompra = dto.DataCompra!.Value
        };
}
