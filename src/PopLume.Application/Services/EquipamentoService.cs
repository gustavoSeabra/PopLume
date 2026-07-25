using Microsoft.Extensions.Logging;
using PopLume.Application.Dtos;
using PopLume.Application.Mappers;
using PopLume.Application.Services.Interfaces;
using PopLume.Domain.Repositories;

namespace PopLume.Application.Services;

public class EquipamentoService(IEquipamentoRepository equipamentoRepository, ILogger<EquipamentoService> logger) : IEquipamentoService
{
    public async Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando consulta de todos os equipamentos ativos.");

            var equipamentos = await equipamentoRepository.ObterTodosEquipamentosAsync(cancellationToken);
            var dtos = equipamentos.Select(e => e.ToDto());

            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter todos os equipamentos.");
            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaErro("Ocorreu um erro ao recuperar a lista de equipamentos.");
        }
    }

    public async Task<ResultadoDto<EquipamentoDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Buscando equipamento com ID: {EquipamentoId}", id);

            var equipamento = await equipamentoRepository.ObterEquipamentosPorIdAsync(id, cancellationToken);

            if (equipamento == null)
            {
                logger.LogWarning("Equipamento {EquipamentoId} não encontrado.", id);
                return ResultadoDto<EquipamentoDto?>.RetornaNaoEncontrado("Equipamento não encontrado.");
            }

            return ResultadoDto<EquipamentoDto?>.RetornaSucesso(equipamento.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar equipamento com ID: {EquipamentoId}", id);
            return ResultadoDto<EquipamentoDto?>.RetornaErro($"Erro ao buscar os detalhes do equipamento {id}.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Pesquisando equipamentos pelo nome: {NomeBusca}", nome);

            var equipamentos = await equipamentoRepository.ObterEquipamentosPorNomeAsync(nome, cancellationToken);
            var dtos = equipamentos.Select(e => e.ToDto());

            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao pesquisar equipamentos pelo nome: {NomeBusca}", nome);
            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaErro("Erro ao realizar a busca por nome.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterPorApelidoAsync(string apelido, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Pesquisando equipamentos pelo apelido: {ApelidoBusca}", apelido);

            var equipamentos = await equipamentoRepository.ObterEquipamentosPorApelidoAsync(apelido, cancellationToken);
            var dtos = equipamentos.Select(e => e.ToDto());

            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao pesquisar equipamentos pelo apelido: {ApelidoBusca}", apelido);
            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaErro("Erro ao realizar a busca por apelido.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterDesativadosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando consulta de equipamentos desativados.");

            var equipamentos = await equipamentoRepository.ObterEquipamentosDesativadosAsync(cancellationToken);
            var dtos = equipamentos.Select(e => e.ToDto());

            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter equipamentos desativados.");
            return ResultadoDto<IEnumerable<EquipamentoDto>>.RetornaErro("Ocorreu um erro ao recuperar a lista de equipamentos desativados.");
        }
    }

    public async Task<ResultadoDto<Guid>> AdicionarAsync(CreateEquipamentoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Tentando adicionar novo equipamento: {NomeEquipamento}", dto.Nome);

            var equipamento = dto.ToEntity();
            equipamentoRepository.Adicionar(equipamento);
            await equipamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Equipamento criado com sucesso. ID: {EquipamentoId}", equipamento.IdEquipamento);
            return ResultadoDto<Guid>.RetornaSucesso(equipamento.IdEquipamento);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar equipamento: {NomeEquipamento}", dto.Nome);
            return ResultadoDto<Guid>.RetornaErro("Não foi possível salvar o equipamento.");
        }
    }

    public async Task<ResultadoDto<bool>> AtualizarAsync(UpdateEquipamentoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando atualização do equipamento {EquipamentoId}.", dto.IdEquipamento);

            var equipamentoExistente = await equipamentoRepository.ObterEquipamentosPorIdAsync(dto.IdEquipamento, cancellationToken);

            if (equipamentoExistente == null)
            {
                logger.LogWarning("Falha na atualização: Equipamento {EquipamentoId} inexistente.", dto.IdEquipamento);
                return ResultadoDto<bool>.RetornaNaoEncontrado("Equipamento não encontrado para atualização.");
            }

            if (!string.IsNullOrEmpty(dto.Nome))
                equipamentoExistente.Nome = dto.Nome;
            if (!string.IsNullOrEmpty(dto.Apelido))
                equipamentoExistente.Apelido = dto.Apelido;
            if (dto.DataCompra.HasValue)
                equipamentoExistente.DataCompra = dto.DataCompra.Value;
            if (dto.Potencia.HasValue)
                equipamentoExistente.Potencia = dto.Potencia.Value;
            if (dto.ValorCompra.HasValue)
                equipamentoExistente.ValorCompra = dto.ValorCompra.Value;
            if (dto.ExpectativaVida.HasValue)
                equipamentoExistente.ExpectativaVida = dto.ExpectativaVida.Value;

            equipamentoRepository.Atualizar(equipamentoExistente);
            await equipamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Equipamento {EquipamentoId} atualizado com sucesso.", dto.IdEquipamento);
            return ResultadoDto<bool>.RetornaSucesso("Equipamento atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar equipamento {EquipamentoId}.", dto.IdEquipamento);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a atualização do equipamento.");
        }
    }

    public async Task<ResultadoDto<bool>> DesativarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando desativação do equipamento {EquipamentoId}.", id);

            var equipamento = await equipamentoRepository.ObterEquipamentosPorIdAsync(id, cancellationToken);

            if (equipamento == null)
            {
                logger.LogWarning("Falha na desativação: Equipamento {EquipamentoId} inexistente.", id);
                return ResultadoDto<bool>.RetornaNaoEncontrado("Equipamento não encontrado para desativação.");
            }

            equipamentoRepository.Remover(equipamento);
            await equipamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Equipamento {EquipamentoId} desativado com sucesso.", id);
            return ResultadoDto<bool>.RetornaSucesso("Equipamento desativado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao desativar equipamento {EquipamentoId}.", id);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a desativação do equipamento.");
        }
    }

    public async Task<ResultadoDto<bool>> RestaurarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando restauração do equipamento {EquipamentoId}.", id);

            var equipamento = await equipamentoRepository.ObterEquipamentoDesativadoPorIdAsync(id, cancellationToken);

            if (equipamento == null)
            {
                logger.LogWarning("Falha na restauração: Equipamento {EquipamentoId} desativado não encontrado.", id);
                return ResultadoDto<bool>.RetornaNaoEncontrado("Equipamento desativado não encontrado para restauração.");
            }

            equipamento.Restaurar();
            equipamentoRepository.Atualizar(equipamento);
            await equipamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Equipamento {EquipamentoId} restaurado com sucesso.", id);
            return ResultadoDto<bool>.RetornaSucesso("Equipamento restaurado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao restaurar equipamento {EquipamentoId}.", id);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a restauração do equipamento.");
        }
    }
}
