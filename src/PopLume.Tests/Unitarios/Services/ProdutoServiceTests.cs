using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PopLume.Application.Services;
using PopLume.Domain.Entities;
using PopLume.Domain.Repositories;
using PopLume.Tests.Mocks.Dtos;
using Xunit;

namespace PopLume.Tests.Unitarios.Services;

public class ProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<ProdutoService>> _loggerMock = new();

    private ProdutoService CriarService()
    {
        _produtoRepositoryMock
            .Setup(repository => repository.UnitOfWork)
            .Returns(_unitOfWorkMock.Object);

        return new ProdutoService(_produtoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Deve listar todos os produtos retornados pelo repositório.")]
    public async Task ObterTodosAsync_DeveRetornarTodosOsProdutos()
    {
        // Arrange
        var produtos = ProdutoDtoMock.ProdutosValidos();

        _produtoRepositoryMock
            .Setup(repository => repository.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        var service = CriarService();

        // Act
        var resultado = await service.ObterTodosAsync();

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(produtos.Count);
        resultado.Data.Should().OnlyContain(produtoDto =>
            produtos.Any(produto => produto.IdProduto == produtoDto.IdProduto));
    }

    [Fact(DisplayName = "Deve listar produtos filtrados por parte do nome informado.")]
    public async Task ObterPorNomeAsync_DeveRetornarProdutosQueContenhamParteDoNome()
    {
        // Arrange
        const string parteNome = "Chaveiro";
        var produtos = new List<Produto>
        {
            ProdutoDtoMock.ProdutoValido(nome: "Chaveiro Personalizado"),
            ProdutoDtoMock.ProdutoValido(nome: "Mini Chaveiro 3D")
        };

        _produtoRepositoryMock
            .Setup(repository => repository.ObterProdutosPorNomeAsync(parteNome, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorNomeAsync(parteNome);

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(2);
        resultado.Data.Should().OnlyContain(produto => produto.Nome.Contains(parteNome, StringComparison.OrdinalIgnoreCase));

        _produtoRepositoryMock.Verify(
            repository => repository.ObterProdutosPorNomeAsync(parteNome, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Deve obter um produto pelo identificador quando ele existir.")]
    public async Task ObterPorIdAsync_DeveRetornarProduto_QuandoIdentificadorExistir()
    {
        // Arrange
        var produto = ProdutoDtoMock.ProdutoValido();

        _produtoRepositoryMock
            .Setup(repository => repository.ObterProdutosPorIdAsync(produto.IdProduto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produto);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorIdAsync(produto.IdProduto);

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.NotFound.Should().BeFalse();
        resultado.Data.Should().NotBeNull();
        resultado.Data!.IdProduto.Should().Be(produto.IdProduto);
        resultado.Data.Nome.Should().Be(produto.Nome);
    }

    [Fact(DisplayName = "Deve cadastrar um produto sem componentes.")]
    public async Task AdicionarAsync_DeveCadastrarProdutoSemComponentes()
    {
        // Arrange
        var dto = ProdutoDtoMock.CreateProdutoDtoSemComponentes();
        Produto? produtoAdicionado = null;

        _produtoRepositoryMock
            .Setup(repository => repository.Adicionar(It.IsAny<Produto>()))
            .Callback<Produto>(produto => produtoAdicionado = produto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AdicionarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        produtoAdicionado.Should().NotBeNull();
        produtoAdicionado!.Nome.Should().Be(dto.Nome);
        produtoAdicionado.ComposicoesPai.Should().BeEmpty();
    }

    [Fact(DisplayName = "Deve cadastrar um produto com componentes.")]
    public async Task AdicionarAsync_DeveCadastrarProdutoComComponentes()
    {
        // Arrange
        var dto = ProdutoDtoMock.CreateProdutoDtoComComponentes();
        Produto? produtoAdicionado = null;

        _produtoRepositoryMock
            .Setup(repository => repository.Adicionar(It.IsAny<Produto>()))
            .Callback<Produto>(produto => produtoAdicionado = produto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AdicionarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        produtoAdicionado.Should().NotBeNull();
        produtoAdicionado!.ComposicoesPai.Should().HaveCount(dto.Componentes!.Count);
        produtoAdicionado.ComposicoesPai.Select(composicao => composicao.IdProdutoFilho)
            .Should().BeEquivalentTo(dto.Componentes.Select(componente => componente.IdProdutoFilho));
    }

    [Fact(DisplayName = "Deve manter quantidade de filamento e tempo de impressão zerados ao cadastrar produto sem componentes.")]
    public async Task AdicionarAsync_DeveManterQuantidadeFilamentoETempoImpressaoZerados_QuandoProdutoNaoPossuirComponentes()
    {
        // Arrange
        var dto = ProdutoDtoMock.CreateProdutoDtoSemComponentes();
        Produto? produtoAdicionado = null;

        _produtoRepositoryMock
            .Setup(repository => repository.Adicionar(It.IsAny<Produto>()))
            .Callback<Produto>(produto => produtoAdicionado = produto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        await service.AdicionarAsync(dto);

        // Assert
        produtoAdicionado.Should().NotBeNull();
        produtoAdicionado!.QuantidadeFilamento.Should().Be(0);
        produtoAdicionado.TempoImpressao.Should().Be(0);
    }

    [Fact(DisplayName = "Deve editar um produto sem componentes.")]
    public async Task AtualizarAsync_DeveEditarProdutoSemComponentes()
    {
        // Arrange
        var produtoExistente = ProdutoDtoMock.ProdutoValido();
        var dto = ProdutoDtoMock.UpdateProdutoDtoSemComponentes(produtoExistente.IdProduto);
        Produto? produtoAtualizado = null;

        _produtoRepositoryMock
            .Setup(repository => repository.ObterProdutosPorIdAsync(dto.IdProduto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtoExistente);

        _produtoRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Produto>()))
            .Callback<Produto>(produto => produtoAtualizado = produto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        produtoAtualizado.Should().NotBeNull();
        produtoAtualizado!.Nome.Should().Be(dto.Nome);
        produtoAtualizado.PrecoCusto.Should().Be(dto.PrecoCusto);
        produtoAtualizado.ComposicoesPai.Should().BeEmpty();
    }

    [Fact(DisplayName = "Deve editar um produto com componentes.")]
    public async Task AtualizarAsync_DeveEditarProdutoComComponentes()
    {
        // Arrange
        var produtoExistente = ProdutoDtoMock.ProdutoValido();
        var dto = ProdutoDtoMock.UpdateProdutoDtoComComponentes(produtoExistente.IdProduto);
        Produto? produtoAtualizado = null;

        _produtoRepositoryMock
            .Setup(repository => repository.ObterProdutosPorIdAsync(dto.IdProduto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtoExistente);

        _produtoRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Produto>()))
            .Callback<Produto>(produto => produtoAtualizado = produto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        produtoAtualizado.Should().NotBeNull();
        produtoAtualizado!.Nome.Should().Be(dto.Nome);
        produtoAtualizado.ComposicoesPai.Should().HaveCount(dto.Componentes!.Count);
        produtoAtualizado.ComposicoesPai.Select(composicao => composicao.IdProdutoFilho)
            .Should().BeEquivalentTo(dto.Componentes.Select(componente => componente.IdProdutoFilho));
    }

    [Fact(DisplayName = "Deve manter quantidade de filamento e tempo de impressão zerados ao editar produto sem componentes.")]
    public async Task AtualizarAsync_DeveManterQuantidadeFilamentoETempoImpressaoZerados_QuandoProdutoNaoPossuirComponentes()
    {
        // Arrange
        var produtoExistente = ProdutoDtoMock.ProdutoValido();
        var dto = ProdutoDtoMock.UpdateProdutoDtoSemComponentes(produtoExistente.IdProduto);
        Produto? produtoAtualizado = null;

        _produtoRepositoryMock
            .Setup(repository => repository.ObterProdutosPorIdAsync(dto.IdProduto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtoExistente);

        _produtoRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Produto>()))
            .Callback<Produto>(produto => produtoAtualizado = produto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        await service.AtualizarAsync(dto);

        // Assert
        produtoAtualizado.Should().NotBeNull();
        produtoAtualizado!.QuantidadeFilamento.Should().Be(0);
        produtoAtualizado.TempoImpressao.Should().Be(0);
    }
}
