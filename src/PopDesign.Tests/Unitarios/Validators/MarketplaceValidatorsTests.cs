using FluentAssertions;
using PopDesign.Application.Dtos;
using PopDesign.Application.Validators;
using PopDesign.Tests.Mocks.Dtos;
using Xunit;

namespace PopDesign.Tests.Unitarios.Validators;

public class MarketplaceValidatorsTests
{
    [Fact(DisplayName = "Deve aceitar marketplace sem taxas.")]
    public void CreateMarketplaceValidator_DeveAceitarMarketplaceSemTaxas()
    {
        // Arrange
        var validator = new CreateMarketplaceValidator();
        var dto = MarketplaceDtoMock.CreateMarketplaceDtoValido(quantidadeTaxas: 0);

        // Act
        var resultado = validator.Validate(dto);

        // Assert
        resultado.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve exigir os campos obrigatórios de uma nova taxa.")]
    public void CreateMarketplaceTaxaValidator_DeveExigirCamposObrigatorios()
    {
        // Arrange
        var validator = new CreateMarketplaceTaxaValidator();
        var remocoes = new Action<CreateMarketplaceTaxaDto>[]
        {
            dto => dto.ValorInicial = null,
            dto => dto.ValorFinal = null,
            dto => dto.TaxaFixa = null
        };

        foreach (var removerCampo in remocoes)
        {
            var dto = MarketplaceDtoMock.CreateMarketplaceTaxaDtoValida();
            removerCampo(dto);

            // Act
            var resultado = validator.Validate(dto);

            // Assert
            resultado.IsValid.Should().BeFalse();
        }
    }

    [Fact(DisplayName = "Deve aceitar comissão não informada.")]
    public void CreateMarketplaceTaxaValidator_DeveAceitarComissaoNaoInformada()
    {
        // Arrange
        var validator = new CreateMarketplaceTaxaValidator();
        var dto = MarketplaceDtoMock.CreateMarketplaceTaxaDtoValida();
        dto.Comissao = null;

        // Act
        var resultado = validator.Validate(dto);

        // Assert
        resultado.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve exigir os campos obrigatórios ao incluir taxa durante uma atualização.")]
    public void UpdateMarketplaceTaxaValidator_DeveExigirCamposObrigatoriosParaNovaTaxa()
    {
        // Arrange
        var validator = new UpdateMarketplaceTaxaValidator();
        var remocoes = new Action<UpdateMarketplaceTaxaDto>[]
        {
            dto => dto.ValorInicial = null,
            dto => dto.ValorFinal = null,
            dto => dto.TaxaFixa = null
        };

        foreach (var removerCampo in remocoes)
        {
            var dto = MarketplaceDtoMock.UpdateMarketplaceTaxaDtoValida();
            removerCampo(dto);

            // Act
            var resultado = validator.Validate(dto);

            // Assert
            resultado.IsValid.Should().BeFalse();
        }
    }

    [Fact(DisplayName = "Deve permitir atualização parcial de uma taxa existente.")]
    public void UpdateMarketplaceTaxaValidator_DeveAceitarAtualizacaoParcialDeTaxaExistente()
    {
        // Arrange
        var validator = new UpdateMarketplaceTaxaValidator();
        var dto = new UpdateMarketplaceTaxaDto
        {
            IdTaxa = Guid.NewGuid(),
            Comissao = 2.75m
        };

        // Act
        var resultado = validator.Validate(dto);

        // Assert
        resultado.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve aceitar comissão com casas decimais.")]
    public void CreateMarketplaceTaxaValidator_DeveAceitarComissaoDecimal()
    {
        // Arrange
        var validator = new CreateMarketplaceTaxaValidator();
        var dto = MarketplaceDtoMock.CreateMarketplaceTaxaDtoValida();
        dto.Comissao = 2.75m;

        // Act
        var resultado = validator.Validate(dto);

        // Assert
        resultado.Errors.Should()
            .NotContain(erro => erro.PropertyName == nameof(dto.Comissao));
    }

    [Fact(DisplayName = "Deve rejeitar comissão decimal fora do intervalo permitido.")]
    public void CreateMarketplaceTaxaValidator_DeveRejeitarComissaoForaDoIntervalo()
    {
        // Arrange
        var validator = new CreateMarketplaceTaxaValidator();
        var comissoesInvalidas = new[] { -0.01m, 100.01m };

        foreach (var comissao in comissoesInvalidas)
        {
            var dto = MarketplaceDtoMock.CreateMarketplaceTaxaDtoValida();
            dto.Comissao = comissao;

            // Act
            var resultado = validator.Validate(dto);

            // Assert
            resultado.Errors.Should()
                .Contain(erro => erro.PropertyName == nameof(dto.Comissao));
        }
    }
}
