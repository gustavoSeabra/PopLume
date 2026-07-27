using FluentAssertions;
using PopLume.Application.Dtos;
using PopLume.Application.Validators;
using Xunit;

namespace PopLume.Tests.Unitarios.Validators;

public class EquipamentoValidatorsTests
{
    [Fact(DisplayName = "Deve aceitar expectativa de vida informada em horas na criação.")]
    public void CreateEquipamentoValidator_DeveAceitarExpectativaVidaEmHoras()
    {
        // Arrange
        var validator = new CreateEquipamentoValidator();
        var dto = new CreateEquipamentoDto
        {
            Nome = "Impressora 3D",
            DataCompra = DateOnly.FromDateTime(DateTime.Today),
            Potencia = 220,
            ValorCompra = 4000m,
            ExpectativaVida = 2000
        };

        // Act
        var resultado = validator.Validate(dto);

        // Assert
        resultado.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Deve rejeitar expectativa de vida sem horas válidas na criação.")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateEquipamentoValidator_DeveRejeitarExpectativaVidaInvalida(int expectativaVida)
    {
        // Arrange
        var validator = new CreateEquipamentoValidator();
        var dto = new CreateEquipamentoDto
        {
            Nome = "Impressora 3D",
            DataCompra = DateOnly.FromDateTime(DateTime.Today),
            Potencia = 220,
            ValorCompra = 4000m,
            ExpectativaVida = expectativaVida
        };

        // Act
        var resultado = validator.Validate(dto);

        // Assert
        resultado.Errors.Should().ContainSingle(erro =>
            erro.PropertyName == nameof(CreateEquipamentoDto.ExpectativaVida) &&
            erro.ErrorMessage == "A expectativa de vida em horas deve ser maior que zero.");
    }

    [Theory(DisplayName = "Deve rejeitar expectativa de vida sem horas válidas na atualização.")]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateEquipamentoValidator_DeveRejeitarExpectativaVidaInvalida(int expectativaVida)
    {
        // Arrange
        var validator = new UpdateEquipamentoValidator();
        var dto = new UpdateEquipamentoDto
        {
            IdEquipamento = Guid.NewGuid(),
            Nome = "Impressora 3D",
            Potencia = 220,
            ValorCompra = 4000m,
            ExpectativaVida = expectativaVida
        };

        // Act
        var resultado = validator.Validate(dto);

        // Assert
        resultado.Errors.Should().ContainSingle(erro =>
            erro.PropertyName == nameof(UpdateEquipamentoDto.ExpectativaVida) &&
            erro.ErrorMessage == "A expectativa de vida em horas deve ser maior que zero.");
    }
}
