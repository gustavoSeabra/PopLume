using Microsoft.Extensions.Logging;
using PopLume.Application.Dtos;
using PopLume.Application.Mappers;
using PopLume.Application.Services.Interfaces;
using PopLume.Domain.Enums;
using PopLume.Domain.Repositories;

namespace PopLume.Application.Services;

public class FilamentoService(
    IFilamentoRepository filamentoRepository,
    ILogger<FilamentoService> logger) : IFilamentoService
{
    public async Task<ResultadoDto<IEnumerable<FilamentoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var filamentos = await filamentoRepository.ObterTodosFilamentosAsync(cancellationToken);
            return ResultadoDto<IEnumerable<FilamentoDto>>.RetornaSucesso(filamentos.Select(f => f.ToDto()));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter todos os filamentos.");
            return ResultadoDto<IEnumerable<FilamentoDto>>.RetornaErro("Ocorreu um erro ao recuperar a lista de filamentos.");
        }
    }

    public async Task<ResultadoDto<FilamentoDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var filamento = await filamentoRepository.ObterFilamentoPorIdAsync(id, cancellationToken);
            return filamento == null
                ? ResultadoDto<FilamentoDto?>.RetornaNaoEncontrado("Filamento não encontrado.")
                : ResultadoDto<FilamentoDto?>.RetornaSucesso(filamento.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar filamento com ID: {FilamentoId}", id);
            return ResultadoDto<FilamentoDto?>.RetornaErro($"Erro ao buscar os detalhes do filamento {id}.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<FilamentoDto>>> ObterPorCorAsync(string cor, CancellationToken cancellationToken = default)
    {
        try
        {
            var filamentos = await filamentoRepository.ObterFilamentosPorCorAsync(cor, cancellationToken);
            return ResultadoDto<IEnumerable<FilamentoDto>>.RetornaSucesso(filamentos.Select(f => f.ToDto()));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao pesquisar filamentos pela cor: {Cor}", cor);
            return ResultadoDto<IEnumerable<FilamentoDto>>.RetornaErro("Erro ao realizar a busca por cor.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<FilamentoDto>>> ObterPorTipoAsync(TipoFilamento tipo, CancellationToken cancellationToken = default)
    {
        try
        {
            var filamentos = await filamentoRepository.ObterFilamentosPorTipoAsync(tipo, cancellationToken);
            return ResultadoDto<IEnumerable<FilamentoDto>>.RetornaSucesso(filamentos.Select(f => f.ToDto()));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao pesquisar filamentos pelo tipo: {Tipo}", tipo);
            return ResultadoDto<IEnumerable<FilamentoDto>>.RetornaErro("Erro ao realizar a busca por tipo.");
        }
    }

    public async Task<ResultadoDto<Guid>> AdicionarAsync(CreateFilamentoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var filamento = dto.ToEntity();
            filamentoRepository.Adicionar(filamento);
            await filamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return ResultadoDto<Guid>.RetornaSucesso(filamento.IdFilamento);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar filamento da cor {Cor}.", dto.Cor);
            return ResultadoDto<Guid>.RetornaErro("Não foi possível salvar o filamento.");
        }
    }

    public async Task<ResultadoDto<bool>> AtualizarAsync(UpdateFilamentoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var filamento = await filamentoRepository.ObterFilamentoPorIdAsync(dto.IdFilamento, cancellationToken);
            if (filamento == null)
                return ResultadoDto<bool>.RetornaNaoEncontrado("Filamento não encontrado para atualização.");

            filamento.Cor = dto.Cor;
            filamento.Valor = dto.Valor!.Value;
            filamento.Peso = dto.Peso!.Value;
            filamento.Tipo = dto.Tipo!.Value;
            filamento.DataCompra = dto.DataCompra!.Value;

            filamentoRepository.Atualizar(filamento);
            await filamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return ResultadoDto<bool>.RetornaSucesso("Filamento atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar filamento {FilamentoId}.", dto.IdFilamento);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a atualização do filamento.");
        }
    }

    public async Task<ResultadoDto<bool>> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var filamento = await filamentoRepository.ObterFilamentoPorIdAsync(id, cancellationToken);
            if (filamento == null)
                return ResultadoDto<bool>.RetornaNaoEncontrado("Filamento não encontrado para remoção.");

            filamentoRepository.Remover(filamento);
            await filamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return ResultadoDto<bool>.RetornaSucesso("Filamento removido com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao remover filamento {FilamentoId}.", id);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a remoção do filamento.");
        }
    }
}
