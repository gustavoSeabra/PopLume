using FluentAssertions;
using PopLume.Application.Mappers;
using PopLume.Domain.Entities;
using Xunit;

namespace PopLume.Tests.Unitarios.Mappers;

public class EquipamentoMapperTests
{
    [Fact(DisplayName = "Deve mapear o valor por hora calculado para o DTO.")]
    public void ToDto_DeveMapearValorHoraCalculado()
    {
        // Arrange
        const decimal valorHora = 2m;
        var equipamento = new Equipamento
        {
            ValorCompra = 4000m,
            ExpectativaVida = 2000
        };

        typeof(Equipamento)
            .GetProperty(nameof(Equipamento.ValorHora))!
            .SetValue(equipamento, valorHora);

        // Act
        var dto = equipamento.ToDto();

        // Assert
        dto.ValorHora.Should().Be(valorHora);
    }
}
