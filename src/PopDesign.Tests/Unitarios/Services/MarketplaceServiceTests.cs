using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PopDesign.Application.Dtos;
using PopDesign.Application.Services;
using PopDesign.Domain.Entities;
using PopDesign.Domain.Repositories;
using PopDesign.Tests.Mocks.Dtos;
using Xunit;

namespace PopDesign.Tests.Unitarios.Services;

public class MarketplaceServiceTests
{
    private readonly Mock<IMarketplaceRepository> _marketplaceRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<MarketplaceService>> _loggerMock = new();

    private MarketplaceService CriarService()
    {
        _marketplaceRepositoryMock
            .Setup(repository => repository.UnitOfWork)
            .Returns(_unitOfWorkMock.Object);

        return new MarketplaceService(_marketplaceRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Deve listar todos os marketplaces retornados pelo repositório.")]
    public async Task ObterTodosAsync_DeveRetornarTodosOsMarketplaces()
    {
        // Arrange
        var marketplaces = MarketplaceDtoMock.MarketplacesValidos();

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterTodosMarketplacesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaces);

        var service = CriarService();

        // Act
        var resultado = await service.ObterTodosAsync();

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(marketplaces.Count);
        resultado.Data.Should().OnlyContain(marketplaceDto =>
            marketplaces.Any(marketplace => marketplace.IdMarketplace == marketplaceDto.IdMarketplace));
    }

    [Fact(DisplayName = "Deve listar marketplaces filtrados por parte do nome informado.")]
    public async Task ObterPorNomeAsync_DeveRetornarMarketplacesQueContenhamParteDoNome()
    {
        // Arrange
        const string parteNome = "Shop";
        var marketplaces = new List<Marketplace>
        {
            MarketplaceDtoMock.MarketplaceValido(nome: "Pop Shop"),
            MarketplaceDtoMock.MarketplaceValido(nome: "Design Shop")
        };

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacesPorNomeAsync(parteNome, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaces);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorNomeAsync(parteNome);

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(2);
        resultado.Data.Should().OnlyContain(marketplace => marketplace.Nome.Contains(parteNome, StringComparison.OrdinalIgnoreCase));

        _marketplaceRepositoryMock.Verify(
            repository => repository.ObterMarketplacesPorNomeAsync(parteNome, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Deve obter um marketplace pelo identificador quando ele existir.")]
    public async Task ObterPorIdAsync_DeveRetornarMarketplace_QuandoIdentificadorExistir()
    {
        // Arrange
        var marketplace = MarketplaceDtoMock.MarketplaceValido();

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdAsync(marketplace.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplace);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorIdAsync(marketplace.IdMarketplace);

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.NotFound.Should().BeFalse();
        resultado.Data.Should().NotBeNull();
        resultado.Data!.IdMarketplace.Should().Be(marketplace.IdMarketplace);
        resultado.Data.Nome.Should().Be(marketplace.Nome);
        resultado.Data.TaxasMarketplace.Should().HaveCount(marketplace.TaxasMarketplace.Count);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao buscar marketplace inexistente.")]
    public async Task ObterPorIdAsync_DeveRetornarNaoEncontrado_QuandoIdentificadorNaoExistir()
    {
        // Arrange
        var idMarketplace = Guid.NewGuid();

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdAsync(idMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Marketplace?)null);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorIdAsync(idMarketplace);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();
        resultado.Data.Should().BeNull();
    }

    [Fact(DisplayName = "Deve listar marketplaces desativados.")]
    public async Task ObterDesativadosAsync_DeveRetornarMarketplacesDesativados()
    {
        // Arrange
        var marketplaces = MarketplaceDtoMock.MarketplacesDesativados();

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacesDesativadosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaces);

        var service = CriarService();

        // Act
        var resultado = await service.ObterDesativadosAsync();

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(marketplaces.Count);
        resultado.Data.Should().OnlyContain(marketplace => marketplace.Excluido);
    }

    [Fact(DisplayName = "Deve cadastrar um marketplace com suas taxas.")]
    public async Task AdicionarAsync_DeveCadastrarMarketplaceComTaxas()
    {
        // Arrange
        var dto = MarketplaceDtoMock.CreateMarketplaceDtoValido(quantidadeTaxas: 2);
        Marketplace? marketplaceAdicionado = null;

        _marketplaceRepositoryMock
            .Setup(repository => repository.Adicionar(It.IsAny<Marketplace>()))
            .Callback<Marketplace>(marketplace => marketplaceAdicionado = marketplace);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AdicionarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        marketplaceAdicionado.Should().NotBeNull();
        marketplaceAdicionado!.Nome.Should().Be(dto.Nome);
        marketplaceAdicionado.LinkLoja.Should().Be(dto.LinkLoja);
        marketplaceAdicionado.Excluido.Should().BeFalse();
        marketplaceAdicionado.TaxasMarketplace.Should().HaveCount(dto.TaxasMarketplace!.Count);

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve editar um marketplace sem alterar suas taxas quando a lista de taxas não for informada.")]
    public async Task AtualizarAsync_DeveEditarMarketplaceSemAlterarTaxas_QuandoTaxasNaoForemInformadas()
    {
        // Arrange
        var marketplaceExistente = MarketplaceDtoMock.MarketplaceValido(quantidadeTaxas: 2);
        var quantidadeTaxasAntesAtualizacao = marketplaceExistente.TaxasMarketplace.Count;
        var dto = MarketplaceDtoMock.UpdateMarketplaceDtoValido(marketplaceExistente.IdMarketplace, taxas: null);
        Marketplace? marketplaceAtualizado = null;

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdParaAtualizacaoAsync(dto.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaceExistente);

        _marketplaceRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Marketplace>()))
            .Callback<Marketplace>(marketplace => marketplaceAtualizado = marketplace);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        marketplaceAtualizado.Should().NotBeNull();
        marketplaceAtualizado!.Nome.Should().Be(dto.Nome);
        marketplaceAtualizado.LinkLoja.Should().Be(dto.LinkLoja);
        marketplaceAtualizado.TaxasMarketplace.Should().HaveCount(quantidadeTaxasAntesAtualizacao);

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve sincronizar taxas ao editar um marketplace.")]
    public async Task AtualizarAsync_DeveSincronizarTaxasDoMarketplace()
    {
        // Arrange
        var marketplaceExistente = MarketplaceDtoMock.MarketplaceValido(quantidadeTaxas: 2);
        var taxaAtualizada = marketplaceExistente.TaxasMarketplace.First();
        var taxaRemovida = marketplaceExistente.TaxasMarketplace.Last();
        var novaTaxa = MarketplaceDtoMock.UpdateMarketplaceTaxaDtoValida();
        var dtoTaxaAtualizada = MarketplaceDtoMock.UpdateMarketplaceTaxaDtoValida(taxaAtualizada.IdTaxa);
        var dto = MarketplaceDtoMock.UpdateMarketplaceDtoValido(
            marketplaceExistente.IdMarketplace,
            new List<UpdateMarketplaceTaxaDto> { dtoTaxaAtualizada, novaTaxa });

        Marketplace? marketplaceAtualizado = null;

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdParaAtualizacaoAsync(dto.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaceExistente);

        _marketplaceRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Marketplace>()))
            .Callback<Marketplace>(marketplace => marketplaceAtualizado = marketplace);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        marketplaceAtualizado.Should().NotBeNull();
        marketplaceAtualizado!.TaxasMarketplace.Should().HaveCount(2);
        marketplaceAtualizado.TaxasMarketplace.Should().NotContain(taxa => taxa.IdTaxa == taxaRemovida.IdTaxa);

        var taxaAtualizadaResultado = marketplaceAtualizado.TaxasMarketplace.Single(taxa => taxa.IdTaxa == taxaAtualizada.IdTaxa);
        taxaAtualizadaResultado.ValorInicial.Should().Be(dtoTaxaAtualizada.ValorInicial!.Value);
        taxaAtualizadaResultado.ValorFinal.Should().Be(dtoTaxaAtualizada.ValorFinal!.Value);
        taxaAtualizadaResultado.Comissao.Should().Be(dtoTaxaAtualizada.Comissao!.Value);
        taxaAtualizadaResultado.TaxaFixa.Should().Be(dtoTaxaAtualizada.TaxaFixa!.Value);

        marketplaceAtualizado.TaxasMarketplace.Should().Contain(taxa =>
            taxa.IdTaxa == Guid.Empty &&
            taxa.ValorInicial == novaTaxa.ValorInicial!.Value &&
            taxa.ValorFinal == novaTaxa.ValorFinal!.Value &&
            taxa.Comissao == novaTaxa.Comissao!.Value &&
            taxa.TaxaFixa == novaTaxa.TaxaFixa!.Value);

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve remover todas as taxas ao editar marketplace com lista de taxas vazia.")]
    public async Task AtualizarAsync_DeveRemoverTodasAsTaxas_QuandoListaForVazia()
    {
        // Arrange
        var marketplaceExistente = MarketplaceDtoMock.MarketplaceValido(quantidadeTaxas: 2);
        var dto = MarketplaceDtoMock.UpdateMarketplaceDtoValido(marketplaceExistente.IdMarketplace, new List<UpdateMarketplaceTaxaDto>());
        Marketplace? marketplaceAtualizado = null;

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdParaAtualizacaoAsync(dto.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaceExistente);

        _marketplaceRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Marketplace>()))
            .Callback<Marketplace>(marketplace => marketplaceAtualizado = marketplace);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        marketplaceAtualizado.Should().NotBeNull();
        marketplaceAtualizado!.TaxasMarketplace.Should().BeEmpty();

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar erro ao editar marketplace com taxas duplicadas.")]
    public async Task AtualizarAsync_DeveRetornarErro_QuandoTaxasDuplicadasForemInformadas()
    {
        // Arrange
        var marketplaceExistente = MarketplaceDtoMock.MarketplaceValido(quantidadeTaxas: 1);
        var idTaxa = marketplaceExistente.TaxasMarketplace.Single().IdTaxa;
        var dto = MarketplaceDtoMock.UpdateMarketplaceDtoValido(
            marketplaceExistente.IdMarketplace,
            new List<UpdateMarketplaceTaxaDto>
            {
                MarketplaceDtoMock.UpdateMarketplaceTaxaDtoValida(idTaxa),
                MarketplaceDtoMock.UpdateMarketplaceTaxaDtoValida(idTaxa)
            });

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdParaAtualizacaoAsync(dto.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaceExistente);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeFalse();

        _marketplaceRepositoryMock.Verify(repository => repository.Atualizar(It.IsAny<Marketplace>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar erro ao editar marketplace com taxa que não pertença ao marketplace.")]
    public async Task AtualizarAsync_DeveRetornarErro_QuandoTaxaNaoPertencerAoMarketplace()
    {
        // Arrange
        var marketplaceExistente = MarketplaceDtoMock.MarketplaceValido(quantidadeTaxas: 1);
        var dto = MarketplaceDtoMock.UpdateMarketplaceDtoValido(
            marketplaceExistente.IdMarketplace,
            new List<UpdateMarketplaceTaxaDto>
            {
                MarketplaceDtoMock.UpdateMarketplaceTaxaDtoValida(Guid.NewGuid())
            });

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdParaAtualizacaoAsync(dto.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplaceExistente);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeFalse();

        _marketplaceRepositoryMock.Verify(repository => repository.Atualizar(It.IsAny<Marketplace>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao editar marketplace inexistente.")]
    public async Task AtualizarAsync_DeveRetornarNaoEncontrado_QuandoMarketplaceNaoExistir()
    {
        // Arrange
        var dto = MarketplaceDtoMock.UpdateMarketplaceDtoValido();

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdParaAtualizacaoAsync(dto.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Marketplace?)null);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();

        _marketplaceRepositoryMock.Verify(repository => repository.Atualizar(It.IsAny<Marketplace>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve desativar um marketplace.")]
    public async Task DesativarAsync_DeveDesativarMarketplace()
    {
        // Arrange
        var marketplace = MarketplaceDtoMock.MarketplaceValido();
        Marketplace? marketplaceRemovido = null;

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdAsync(marketplace.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplace);

        _marketplaceRepositoryMock
            .Setup(repository => repository.Remover(It.IsAny<Marketplace>()))
            .Callback<Marketplace>(marketplaceCapturado =>
            {
                marketplaceCapturado.Excluir();
                marketplaceRemovido = marketplaceCapturado;
            });

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.DesativarAsync(marketplace.IdMarketplace);

        // Assert
        resultado.Ok.Should().BeTrue();
        marketplaceRemovido.Should().NotBeNull();
        marketplaceRemovido!.Excluido.Should().BeTrue();
        marketplaceRemovido.DataExclusao.Should().NotBeNull();

        _marketplaceRepositoryMock.Verify(repository => repository.Remover(marketplace), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao desativar marketplace inexistente.")]
    public async Task DesativarAsync_DeveRetornarNaoEncontrado_QuandoMarketplaceNaoExistir()
    {
        // Arrange
        var idMarketplace = Guid.NewGuid();

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplacePorIdAsync(idMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Marketplace?)null);

        var service = CriarService();

        // Act
        var resultado = await service.DesativarAsync(idMarketplace);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();

        _marketplaceRepositoryMock.Verify(repository => repository.Remover(It.IsAny<Marketplace>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve restaurar um marketplace desativado.")]
    public async Task RestaurarAsync_DeveRestaurarMarketplaceDesativado()
    {
        // Arrange
        var marketplace = MarketplaceDtoMock.MarketplaceDesativado();
        Marketplace? marketplaceAtualizado = null;

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplaceDesativadoPorIdAsync(marketplace.IdMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync(marketplace);

        _marketplaceRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Marketplace>()))
            .Callback<Marketplace>(marketplaceCapturado => marketplaceAtualizado = marketplaceCapturado);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.RestaurarAsync(marketplace.IdMarketplace);

        // Assert
        resultado.Ok.Should().BeTrue();
        marketplaceAtualizado.Should().NotBeNull();
        marketplaceAtualizado!.Excluido.Should().BeFalse();
        marketplaceAtualizado.DataExclusao.Should().BeNull();

        _marketplaceRepositoryMock.Verify(repository => repository.Atualizar(marketplace), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao restaurar marketplace que não esteja desativado.")]
    public async Task RestaurarAsync_DeveRetornarNaoEncontrado_QuandoMarketplaceDesativadoNaoExistir()
    {
        // Arrange
        var idMarketplace = Guid.NewGuid();

        _marketplaceRepositoryMock
            .Setup(repository => repository.ObterMarketplaceDesativadoPorIdAsync(idMarketplace, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Marketplace?)null);

        var service = CriarService();

        // Act
        var resultado = await service.RestaurarAsync(idMarketplace);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();

        _marketplaceRepositoryMock.Verify(repository => repository.Atualizar(It.IsAny<Marketplace>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
