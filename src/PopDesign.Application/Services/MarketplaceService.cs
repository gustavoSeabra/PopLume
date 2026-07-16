using Microsoft.Extensions.Logging;
using PopDesign.Application.Dtos;
using PopDesign.Application.Mappers;
using PopDesign.Application.Services.Interfaces;
using PopDesign.Domain.Entities;
using PopDesign.Domain.Repositories;

namespace PopDesign.Application.Services;

public class MarketplaceService(
    IMarketplaceRepository marketplaceRepository,
    ILogger<MarketplaceService> logger) : IMarketplaceService
{
    public async Task<ResultadoDto<IEnumerable<MarketplaceDto>>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando consulta de todos os marketplaces ativos.");

            var marketplaces = await marketplaceRepository.ObterTodosMarketplacesAsync(cancellationToken);
            var dtos = marketplaces.Select(m => m.ToDto());

            return ResultadoDto<IEnumerable<MarketplaceDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter todos os marketplaces.");
            return ResultadoDto<IEnumerable<MarketplaceDto>>.RetornaErro("Ocorreu um erro ao recuperar a lista de marketplaces.");
        }
    }

    public async Task<ResultadoDto<MarketplaceDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Buscando marketplace com ID: {MarketplaceId}", id);

            var marketplace = await marketplaceRepository.ObterMarketplacePorIdAsync(id, cancellationToken);

            if (marketplace == null)
            {
                logger.LogWarning("Marketplace {MarketplaceId} não encontrado.", id);
                return ResultadoDto<MarketplaceDto?>.RetornaNaoEncontrado("Marketplace não encontrado.");
            }

            return ResultadoDto<MarketplaceDto?>.RetornaSucesso(marketplace.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar marketplace com ID: {MarketplaceId}", id);
            return ResultadoDto<MarketplaceDto?>.RetornaErro($"Erro ao buscar os detalhes do marketplace {id}.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<MarketplaceDto>>> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Pesquisando marketplaces pelo nome: {NomeBusca}", nome);

            var marketplaces = await marketplaceRepository.ObterMarketplacesPorNomeAsync(nome, cancellationToken);
            var dtos = marketplaces.Select(m => m.ToDto());

            return ResultadoDto<IEnumerable<MarketplaceDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao pesquisar marketplaces pelo nome: {NomeBusca}", nome);
            return ResultadoDto<IEnumerable<MarketplaceDto>>.RetornaErro("Erro ao realizar a busca por nome.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<MarketplaceDto>>> ObterDesativadosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando consulta de marketplaces desativados.");

            var marketplaces = await marketplaceRepository.ObterMarketplacesDesativadosAsync(cancellationToken);
            var dtos = marketplaces.Select(m => m.ToDto());

            return ResultadoDto<IEnumerable<MarketplaceDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter marketplaces desativados.");
            return ResultadoDto<IEnumerable<MarketplaceDto>>.RetornaErro("Ocorreu um erro ao recuperar a lista de marketplaces desativados.");
        }
    }

    public async Task<ResultadoDto<Guid>> AdicionarAsync(CreateMarketplaceDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Tentando adicionar novo marketplace: {NomeMarketplace}", dto.Nome);

            var marketplace = dto.ToEntity();
            marketplaceRepository.Adicionar(marketplace);
            await marketplaceRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Marketplace criado com sucesso. ID: {MarketplaceId}", marketplace.IdMarketplace);
            return ResultadoDto<Guid>.RetornaSucesso(marketplace.IdMarketplace);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar marketplace: {NomeMarketplace}", dto.Nome);
            return ResultadoDto<Guid>.RetornaErro("Não foi possível salvar o marketplace.");
        }
    }

    public async Task<ResultadoDto<bool>> AtualizarAsync(UpdateMarketplaceDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando atualização do marketplace {MarketplaceId}.", dto.IdMarketplace);

            var marketplaceExistente = await marketplaceRepository.ObterMarketplacePorIdParaAtualizacaoAsync(dto.IdMarketplace, cancellationToken);

            if (marketplaceExistente == null)
            {
                logger.LogWarning("Falha na atualização: Marketplace {MarketplaceId} inexistente.", dto.IdMarketplace);
                return ResultadoDto<bool>.RetornaNaoEncontrado("Marketplace não encontrado para atualização.");
            }

            if (!string.IsNullOrEmpty(dto.Nome))
                marketplaceExistente.Nome = dto.Nome;
            if (!string.IsNullOrEmpty(dto.LinkLoja))
                marketplaceExistente.LinkLoja = dto.LinkLoja;

            var resultadoSincronizacaoTaxas = SincronizarTaxasMarketplace(marketplaceExistente, dto.TaxasMarketplace);
            if (resultadoSincronizacaoTaxas != null)
                return resultadoSincronizacaoTaxas;

            await marketplaceRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Marketplace {MarketplaceId} atualizado com sucesso.", dto.IdMarketplace);
            return ResultadoDto<bool>.RetornaSucesso("Marketplace atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar marketplace {MarketplaceId}.", dto.IdMarketplace);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a atualização do marketplace.");
        }
    }

    public async Task<ResultadoDto<bool>> DesativarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando desativação do marketplace {MarketplaceId}.", id);

            var marketplace = await marketplaceRepository.ObterMarketplacePorIdAsync(id, cancellationToken);

            if (marketplace == null)
            {
                logger.LogWarning("Falha na desativação: Marketplace {MarketplaceId} inexistente.", id);
                return ResultadoDto<bool>.RetornaNaoEncontrado("Marketplace não encontrado para desativação.");
            }

            marketplaceRepository.Remover(marketplace);
            await marketplaceRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Marketplace {MarketplaceId} desativado com sucesso.", id);
            return ResultadoDto<bool>.RetornaSucesso("Marketplace desativado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao desativar marketplace {MarketplaceId}.", id);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a desativação do marketplace.");
        }
    }

    public async Task<ResultadoDto<bool>> RestaurarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando restauração do marketplace {MarketplaceId}.", id);

            var marketplace = await marketplaceRepository.ObterMarketplaceDesativadoPorIdAsync(id, cancellationToken);

            if (marketplace == null)
            {
                logger.LogWarning("Falha na restauração: Marketplace {MarketplaceId} desativado não encontrado.", id);
                return ResultadoDto<bool>.RetornaNaoEncontrado("Marketplace desativado não encontrado para restauração.");
            }

            marketplace.Restaurar();
            marketplaceRepository.Atualizar(marketplace);
            await marketplaceRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Marketplace {MarketplaceId} restaurado com sucesso.", id);
            return ResultadoDto<bool>.RetornaSucesso("Marketplace restaurado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao restaurar marketplace {MarketplaceId}.", id);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a restauração do marketplace.");
        }
    }

    private ResultadoDto<bool>? SincronizarTaxasMarketplace(Marketplace marketplace, List<UpdateMarketplaceTaxaDto>? taxasDto)
    {
        if (taxasDto == null)
            return null;

        var idsTaxasInformados = taxasDto
            .Where(t => t.IdTaxa.HasValue && t.IdTaxa.Value != Guid.Empty)
            .Select(t => t.IdTaxa!.Value)
            .ToList();

        if (idsTaxasInformados.Count != idsTaxasInformados.Distinct().Count())
            return ResultadoDto<bool>.RetornaErro("Existem taxas duplicadas na atualização do marketplace.");

        var taxasExistentes = marketplace.TaxasMarketplace.ToDictionary(t => t.IdTaxa);
        var idsTaxasInvalidos = idsTaxasInformados.Where(idTaxa => !taxasExistentes.ContainsKey(idTaxa)).ToList();

        if (idsTaxasInvalidos.Count > 0)
            return ResultadoDto<bool>.RetornaErro("Uma ou mais taxas informadas não pertencem ao marketplace.");

        var idsTaxasMantidas = idsTaxasInformados.ToHashSet();
        var taxasRemovidas = marketplace.TaxasMarketplace
            .Where(t => !idsTaxasMantidas.Contains(t.IdTaxa))
            .ToList();

        foreach (var taxaRemovida in taxasRemovidas)
        {
            marketplace.TaxasMarketplace.Remove(taxaRemovida);
        }

        foreach (var taxaDto in taxasDto)
        {
            if (taxaDto.IdTaxa.HasValue && taxaDto.IdTaxa.Value != Guid.Empty)
            {
                AtualizarTaxa(taxasExistentes[taxaDto.IdTaxa.Value], taxaDto);
                continue;
            }

            marketplace.TaxasMarketplace.Add(new TaxasMarketplace
            {
                IdMarketplace = marketplace.IdMarketplace,
                ValorInicial = taxaDto.ValorInicial ?? 0m,
                ValorFinal = taxaDto.ValorFinal ?? 0m,
                Comissao = taxaDto.Comissao ?? 0m,
                TaxaFixa = taxaDto.TaxaFixa ?? 0m
            });
        }

        return null;
    }

    private static void AtualizarTaxa(TaxasMarketplace taxaExistente, UpdateMarketplaceTaxaDto taxaDto)
    {
        if (taxaDto.ValorInicial.HasValue)
            taxaExistente.ValorInicial = taxaDto.ValorInicial.Value;
        if (taxaDto.ValorFinal.HasValue)
            taxaExistente.ValorFinal = taxaDto.ValorFinal.Value;
        if (taxaDto.Comissao.HasValue)
            taxaExistente.Comissao = taxaDto.Comissao.Value;
        if (taxaDto.TaxaFixa.HasValue)
            taxaExistente.TaxaFixa = taxaDto.TaxaFixa.Value;
    }
}
