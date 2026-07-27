using FluentAssertions;
using PopLume.Application.Mappers;
using PopLume.Domain.Enums;
using PopLume.Tests.Mocks.Dtos;
using Xunit;

namespace PopLume.Tests.Unitarios.Mappers;

public class FilamentoMapperTests
{
    [Fact(DisplayName = "Deve mapear filamento para DTO.")]
    public void ToDto_DeveMapearTodosOsCampos()
    {
        var filamento = FilamentoDtoMock.FilamentoValido();

        var dto = filamento.ToDto();

        dto.Should().BeEquivalentTo(filamento);
    }

    [Fact(DisplayName = "Deve mapear DTO de criação para filamento.")]
    public void ToEntity_DeveMapearPesoEmGramasETipo()
    {
        var dto = FilamentoDtoMock.CreateFilamentoDtoValido();
        dto.Peso = 1000;
        dto.Tipo = TipoFilamento.PLA;

        var filamento = dto.ToEntity();

        filamento.Peso.Should().Be(1000);
        filamento.Tipo.Should().Be(TipoFilamento.PLA);
        filamento.Cor.Should().Be(dto.Cor);
        filamento.Valor.Should().Be(dto.Valor);
        filamento.DataCompra.Should().Be(dto.DataCompra);
    }
}
