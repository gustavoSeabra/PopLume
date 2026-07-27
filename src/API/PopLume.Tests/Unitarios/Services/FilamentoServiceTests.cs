using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PopLume.Application.Services;
using PopLume.Domain.Entities;
using PopLume.Domain.Repositories;
using PopLume.Tests.Mocks.Dtos;
using Xunit;

namespace PopLume.Tests.Unitarios.Services;

public class FilamentoServiceTests
{
    private readonly Mock<IFilamentoRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<FilamentoService>> _loggerMock = new();

    private FilamentoService CriarService()
    {
        _repositoryMock.Setup(repository => repository.UnitOfWork).Returns(_unitOfWorkMock.Object);
        return new FilamentoService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Deve cadastrar um filamento.")]
    public async Task AdicionarAsync_DeveCadastrarFilamento()
    {
        var dto = FilamentoDtoMock.CreateFilamentoDtoValido();
        Filamento? filamentoAdicionado = null;
        _repositoryMock
            .Setup(repository => repository.Adicionar(It.IsAny<Filamento>()))
            .Callback<Filamento>(filamento => filamentoAdicionado = filamento);

        var resultado = await CriarService().AdicionarAsync(dto);

        resultado.Ok.Should().BeTrue();
        filamentoAdicionado.Should().NotBeNull();
        filamentoAdicionado!.Should().BeEquivalentTo(dto);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve atualizar um filamento existente.")]
    public async Task AtualizarAsync_DeveAtualizarFilamento()
    {
        var filamento = FilamentoDtoMock.FilamentoValido();
        var dto = FilamentoDtoMock.UpdateFilamentoDtoValido(filamento.IdFilamento);
        _repositoryMock
            .Setup(repository => repository.ObterFilamentoPorIdAsync(dto.IdFilamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync(filamento);

        var resultado = await CriarService().AtualizarAsync(dto);

        resultado.Ok.Should().BeTrue();
        filamento.Cor.Should().Be(dto.Cor);
        filamento.Valor.Should().Be(dto.Valor);
        filamento.Peso.Should().Be(dto.Peso);
        filamento.Tipo.Should().Be(dto.Tipo);
        filamento.DataCompra.Should().Be(dto.DataCompra);
        _repositoryMock.Verify(repository => repository.Atualizar(filamento), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao remover filamento inexistente.")]
    public async Task RemoverAsync_DeveRetornarNaoEncontradoQuandoFilamentoNaoExistir()
    {
        var id = Guid.NewGuid();
        _repositoryMock
            .Setup(repository => repository.ObterFilamentoPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Filamento?)null);

        var resultado = await CriarService().RemoverAsync(id);

        resultado.NotFound.Should().BeTrue();
        _repositoryMock.Verify(repository => repository.Remover(It.IsAny<Filamento>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
