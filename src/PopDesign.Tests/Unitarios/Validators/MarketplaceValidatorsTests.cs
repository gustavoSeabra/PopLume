using FluentAssertions;
using PopDesign.Application.Validators;
using PopDesign.Tests.Mocks.Dtos;
using Xunit;

namespace PopDesign.Tests.Unitarios.Validators;

public class MarketplaceValidatorsTests
{
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
