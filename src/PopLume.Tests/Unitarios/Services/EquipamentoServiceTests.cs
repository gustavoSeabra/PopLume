using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PopLume.Application.Services;
using PopLume.Domain.Entities;
using PopLume.Domain.Repositories;
using PopLume.Tests.Mocks.Dtos;
using Xunit;

namespace PopLume.Tests.Unitarios.Services;

public class EquipamentoServiceTests
{
    private readonly Mock<IEquipamentoRepository> _equipamentoRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<EquipamentoService>> _loggerMock = new();

    private EquipamentoService CriarService()
    {
        _equipamentoRepositoryMock
            .Setup(repository => repository.UnitOfWork)
            .Returns(_unitOfWorkMock.Object);

        return new EquipamentoService(_equipamentoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Deve listar todos os equipamentos retornados pelo repositório.")]
    public async Task ObterTodosAsync_DeveRetornarTodosOsEquipamentos()
    {
        // Arrange
        var equipamentos = EquipamentoDtoMock.EquipamentosValidos();

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterTodosEquipamentosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamentos);

        var service = CriarService();

        // Act
        var resultado = await service.ObterTodosAsync();

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(equipamentos.Count);
        resultado.Data.Should().OnlyContain(equipamentoDto =>
            equipamentos.Any(equipamento => equipamento.IdEquipamento == equipamentoDto.IdEquipamento));
    }

    [Fact(DisplayName = "Deve listar equipamentos filtrados por parte do nome informado.")]
    public async Task ObterPorNomeAsync_DeveRetornarEquipamentosQueContenhamParteDoNome()
    {
        // Arrange
        const string parteNome = "Impressora";
        var equipamentos = new List<Equipamento>
        {
            EquipamentoDtoMock.EquipamentoValido(nome: "Impressora 3D Grande"),
            EquipamentoDtoMock.EquipamentoValido(nome: "Mini Impressora 3D")
        };

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorNomeAsync(parteNome, It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamentos);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorNomeAsync(parteNome);

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(2);
        resultado.Data.Should().OnlyContain(equipamento => equipamento.Nome.Contains(parteNome, StringComparison.OrdinalIgnoreCase));

        _equipamentoRepositoryMock.Verify(
            repository => repository.ObterEquipamentosPorNomeAsync(parteNome, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Deve listar equipamentos filtrados por parte do apelido informado.")]
    public async Task ObterPorApelidoAsync_DeveRetornarEquipamentosQueContenhamParteDoApelido()
    {
        // Arrange
        const string parteApelido = "Bambu";
        var equipamentos = new List<Equipamento>
        {
            EquipamentoDtoMock.EquipamentoValido(apelido: "Bambu Lab 01"),
            EquipamentoDtoMock.EquipamentoValido(apelido: "Bambu Lab 02")
        };

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorApelidoAsync(parteApelido, It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamentos);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorApelidoAsync(parteApelido);

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(2);
        resultado.Data.Should().OnlyContain(equipamento => equipamento.Apelido.Contains(parteApelido, StringComparison.OrdinalIgnoreCase));

        _equipamentoRepositoryMock.Verify(
            repository => repository.ObterEquipamentosPorApelidoAsync(parteApelido, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Deve obter um equipamento pelo identificador quando ele existir.")]
    public async Task ObterPorIdAsync_DeveRetornarEquipamento_QuandoIdentificadorExistir()
    {
        // Arrange
        var equipamento = EquipamentoDtoMock.EquipamentoValido();

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorIdAsync(equipamento.IdEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamento);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorIdAsync(equipamento.IdEquipamento);

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.NotFound.Should().BeFalse();
        resultado.Data.Should().NotBeNull();
        resultado.Data!.IdEquipamento.Should().Be(equipamento.IdEquipamento);
        resultado.Data.Nome.Should().Be(equipamento.Nome);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao buscar equipamento inexistente.")]
    public async Task ObterPorIdAsync_DeveRetornarNaoEncontrado_QuandoIdentificadorNaoExistir()
    {
        // Arrange
        var idEquipamento = Guid.NewGuid();

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorIdAsync(idEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Equipamento?)null);

        var service = CriarService();

        // Act
        var resultado = await service.ObterPorIdAsync(idEquipamento);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();
        resultado.Data.Should().BeNull();
    }

    [Fact(DisplayName = "Deve listar equipamentos desativados.")]
    public async Task ObterDesativadosAsync_DeveRetornarEquipamentosDesativados()
    {
        // Arrange
        var equipamentos = EquipamentoDtoMock.EquipamentosDesativados();

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosDesativadosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamentos);

        var service = CriarService();

        // Act
        var resultado = await service.ObterDesativadosAsync();

        // Assert
        resultado.Ok.Should().BeTrue();
        resultado.Data.Should().NotBeNull();
        resultado.Data.Should().HaveCount(equipamentos.Count);
        resultado.Data.Should().OnlyContain(equipamento => equipamento.Excluido);
    }

    [Fact(DisplayName = "Deve cadastrar um equipamento.")]
    public async Task AdicionarAsync_DeveCadastrarEquipamento()
    {
        // Arrange
        var dto = EquipamentoDtoMock.CreateEquipamentoDtoValido();
        Equipamento? equipamentoAdicionado = null;

        _equipamentoRepositoryMock
            .Setup(repository => repository.Adicionar(It.IsAny<Equipamento>()))
            .Callback<Equipamento>(equipamento => equipamentoAdicionado = equipamento);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AdicionarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        equipamentoAdicionado.Should().NotBeNull();
        equipamentoAdicionado!.Nome.Should().Be(dto.Nome);
        equipamentoAdicionado.Apelido.Should().Be(dto.Apelido);
        equipamentoAdicionado.DataCompra.Should().Be(dto.DataCompra!.Value);
        equipamentoAdicionado.Potencia.Should().Be(dto.Potencia!.Value);
        equipamentoAdicionado.ValorCompra.Should().Be(dto.ValorCompra!.Value);
        equipamentoAdicionado.ExpectativaVida.Should().Be(dto.ExpectativaVida!.Value);
        equipamentoAdicionado.Excluido.Should().BeFalse();

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve editar um equipamento.")]
    public async Task AtualizarAsync_DeveEditarEquipamento()
    {
        // Arrange
        var equipamentoExistente = EquipamentoDtoMock.EquipamentoValido();
        var dto = EquipamentoDtoMock.UpdateEquipamentoDtoValido(equipamentoExistente.IdEquipamento);
        Equipamento? equipamentoAtualizado = null;

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorIdAsync(dto.IdEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamentoExistente);

        _equipamentoRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Equipamento>()))
            .Callback<Equipamento>(equipamento => equipamentoAtualizado = equipamento);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeTrue();
        equipamentoAtualizado.Should().NotBeNull();
        equipamentoAtualizado!.Nome.Should().Be(dto.Nome);
        equipamentoAtualizado.Apelido.Should().Be(dto.Apelido);
        equipamentoAtualizado.DataCompra.Should().Be(dto.DataCompra!.Value);
        equipamentoAtualizado.Potencia.Should().Be(dto.Potencia!.Value);
        equipamentoAtualizado.ValorCompra.Should().Be(dto.ValorCompra!.Value);
        equipamentoAtualizado.ExpectativaVida.Should().Be(dto.ExpectativaVida!.Value);

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao editar equipamento inexistente.")]
    public async Task AtualizarAsync_DeveRetornarNaoEncontrado_QuandoEquipamentoNaoExistir()
    {
        // Arrange
        var dto = EquipamentoDtoMock.UpdateEquipamentoDtoValido();

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorIdAsync(dto.IdEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Equipamento?)null);

        var service = CriarService();

        // Act
        var resultado = await service.AtualizarAsync(dto);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();

        _equipamentoRepositoryMock.Verify(repository => repository.Atualizar(It.IsAny<Equipamento>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve desativar um equipamento.")]
    public async Task DesativarAsync_DeveDesativarEquipamento()
    {
        // Arrange
        var equipamento = EquipamentoDtoMock.EquipamentoValido();
        Equipamento? equipamentoRemovido = null;

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorIdAsync(equipamento.IdEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamento);

        _equipamentoRepositoryMock
            .Setup(repository => repository.Remover(It.IsAny<Equipamento>()))
            .Callback<Equipamento>(equipamentoCapturado =>
            {
                equipamentoCapturado.Excluir();
                equipamentoRemovido = equipamentoCapturado;
            });

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.DesativarAsync(equipamento.IdEquipamento);

        // Assert
        resultado.Ok.Should().BeTrue();
        equipamentoRemovido.Should().NotBeNull();
        equipamentoRemovido!.Excluido.Should().BeTrue();
        equipamentoRemovido.DataExclusao.Should().NotBeNull();

        _equipamentoRepositoryMock.Verify(repository => repository.Remover(equipamento), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao desativar equipamento inexistente.")]
    public async Task DesativarAsync_DeveRetornarNaoEncontrado_QuandoEquipamentoNaoExistir()
    {
        // Arrange
        var idEquipamento = Guid.NewGuid();

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentosPorIdAsync(idEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Equipamento?)null);

        var service = CriarService();

        // Act
        var resultado = await service.DesativarAsync(idEquipamento);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();

        _equipamentoRepositoryMock.Verify(repository => repository.Remover(It.IsAny<Equipamento>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve restaurar um equipamento desativado.")]
    public async Task RestaurarAsync_DeveRestaurarEquipamentoDesativado()
    {
        // Arrange
        var equipamento = EquipamentoDtoMock.EquipamentoDesativado();
        Equipamento? equipamentoAtualizado = null;

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentoDesativadoPorIdAsync(equipamento.IdEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync(equipamento);

        _equipamentoRepositoryMock
            .Setup(repository => repository.Atualizar(It.IsAny<Equipamento>()))
            .Callback<Equipamento>(equipamentoCapturado => equipamentoAtualizado = equipamentoCapturado);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CriarService();

        // Act
        var resultado = await service.RestaurarAsync(equipamento.IdEquipamento);

        // Assert
        resultado.Ok.Should().BeTrue();
        equipamentoAtualizado.Should().NotBeNull();
        equipamentoAtualizado!.Excluido.Should().BeFalse();
        equipamentoAtualizado.DataExclusao.Should().BeNull();

        _equipamentoRepositoryMock.Verify(repository => repository.Atualizar(equipamento), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar não encontrado ao restaurar equipamento que não esteja desativado.")]
    public async Task RestaurarAsync_DeveRetornarNaoEncontrado_QuandoEquipamentoDesativadoNaoExistir()
    {
        // Arrange
        var idEquipamento = Guid.NewGuid();

        _equipamentoRepositoryMock
            .Setup(repository => repository.ObterEquipamentoDesativadoPorIdAsync(idEquipamento, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Equipamento?)null);

        var service = CriarService();

        // Act
        var resultado = await service.RestaurarAsync(idEquipamento);

        // Assert
        resultado.Ok.Should().BeFalse();
        resultado.NotFound.Should().BeTrue();

        _equipamentoRepositoryMock.Verify(repository => repository.Atualizar(It.IsAny<Equipamento>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
