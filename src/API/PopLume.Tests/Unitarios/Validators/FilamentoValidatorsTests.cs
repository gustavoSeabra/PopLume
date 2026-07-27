using FluentAssertions;
using PopLume.Application.Dtos;
using PopLume.Application.Validators;
using PopLume.Domain.Enums;
using PopLume.Tests.Mocks.Dtos;
using Xunit;

namespace PopLume.Tests.Unitarios.Validators;

public class FilamentoValidatorsTests
{
    [Theory(DisplayName = "Deve aceitar todos os tipos de filamento permitidos.")]
    [InlineData(TipoFilamento.ABS)]
    [InlineData(TipoFilamento.PETG)]
    [InlineData(TipoFilamento.PLA)]
    [InlineData(TipoFilamento.TPU)]
    public void CreateFilamentoValidator_DeveAceitarTipoValido(TipoFilamento tipo)
    {
        var dto = FilamentoDtoMock.CreateFilamentoDtoValido();
        dto.Tipo = tipo;

        var resultado = new CreateFilamentoValidator().Validate(dto);

        resultado.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve rejeitar tipo de filamento não definido.")]
    public void CreateFilamentoValidator_DeveRejeitarTipoInvalido()
    {
        var dto = FilamentoDtoMock.CreateFilamentoDtoValido();
        dto.Tipo = (TipoFilamento)999;

        var resultado = new CreateFilamentoValidator().Validate(dto);

        resultado.Errors.Should().Contain(erro =>
            erro.PropertyName == nameof(CreateFilamentoDto.Tipo));
    }

    [Theory(DisplayName = "Deve rejeitar peso igual a zero ou negativo.")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateFilamentoValidator_DeveRejeitarPesoInvalido(decimal peso)
    {
        var dto = FilamentoDtoMock.CreateFilamentoDtoValido();
        dto.Peso = peso;

        var resultado = new CreateFilamentoValidator().Validate(dto);

        resultado.Errors.Should().Contain(erro =>
            erro.PropertyName == nameof(CreateFilamentoDto.Peso));
    }

    [Fact(DisplayName = "Deve exigir todos os campos na criação.")]
    public void CreateFilamentoValidator_DeveExigirCamposObrigatorios()
    {
        var dto = new CreateFilamentoDto();

        var resultado = new CreateFilamentoValidator().Validate(dto);

        resultado.Errors.Select(erro => erro.PropertyName).Should().Contain(
        [
            nameof(CreateFilamentoDto.Cor),
            nameof(CreateFilamentoDto.Valor),
            nameof(CreateFilamentoDto.Peso),
            nameof(CreateFilamentoDto.Tipo),
            nameof(CreateFilamentoDto.DataCompra)
        ]);
    }

    [Fact(DisplayName = "Deve exigir identificador e campos na atualização.")]
    public void UpdateFilamentoValidator_DeveExigirIdentificadorECampos()
    {
        var dto = new UpdateFilamentoDto();

        var resultado = new UpdateFilamentoValidator().Validate(dto);

        resultado.Errors.Select(erro => erro.PropertyName).Should().Contain(
        [
            nameof(UpdateFilamentoDto.IdFilamento),
            nameof(UpdateFilamentoDto.Cor),
            nameof(UpdateFilamentoDto.Valor),
            nameof(UpdateFilamentoDto.Peso),
            nameof(UpdateFilamentoDto.Tipo),
            nameof(UpdateFilamentoDto.DataCompra)
        ]);
    }
}
