using PopLume.Application.Dtos;
using PopLume.Application.Services.Interfaces;
using PopLume.Domain.Repositories;
using PopLume.Domain.Entities;
using PopLume.Application.Mappers;
using Microsoft.Extensions.Logging;

namespace PopLume.Application.Services;

public class ProdutoService(IProdutoRepository produtoRepository, ILogger<ProdutoService> logger) : IProdutoService
{
    public async Task<ResultadoDto<IEnumerable<ProdutoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando consulta de todos os produtos.");
            
            var produtos = await produtoRepository.ObterTodosProdutosAsync(cancellationToken);
            var dtos = produtos.Select(p => p.ToDto());
            
            return ResultadoDto<IEnumerable<ProdutoDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter todos os produtos.");
            return ResultadoDto<IEnumerable<ProdutoDto>>.RetornaErro("Ocorreu um erro ao recuperar a lista de produtos.");
        }
    }

    public async Task<ResultadoDto<ProdutoDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Buscando produto com ID: {ProdutoId}", id);

            var produto = await produtoRepository.ObterProdutosPorIdAsync(id, cancellationToken);
            
            if (produto == null)
            {
                logger.LogWarning("Produto {ProdutoId} não encontrado.", id);
                return ResultadoDto<ProdutoDto?>.RetornaNaoEncontrado("Produto não encontrado");
            }

            return ResultadoDto<ProdutoDto?>.RetornaSucesso(produto.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar produto com ID: {ProdutoId}", id);
            return ResultadoDto<ProdutoDto?>.RetornaErro($"Erro ao buscar os detalhes do produto {id}.");
        }
    }

    public async Task<ResultadoDto<IEnumerable<ProdutoDto>>> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Pesquisando produtos pelo nome: {NomeBusca}", nome);

            var produtos = await produtoRepository.ObterProdutosPorNomeAsync(nome, cancellationToken);
            var dtos = produtos.Select(p => p.ToDto());

            return ResultadoDto<IEnumerable<ProdutoDto>>.RetornaSucesso(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao pesquisar produtos pelo nome: {NomeBusca}", nome);
            return ResultadoDto<IEnumerable<ProdutoDto>>.RetornaErro("Erro ao realizar a busca por nome.");
        }
    }

    public async Task<ResultadoDto<Guid>> AdicionarAsync(CreateProdutoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Tentando adicionar novo produto: {NomeProduto}", dto.Nome);

            var produto = dto.ToEntity();
            produtoRepository.Adicionar(produto);
            await produtoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Produto criado com sucesso. ID: {ProdutoId}", produto.IdProduto);
            return ResultadoDto<Guid>.RetornaSucesso(produto.IdProduto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar produto: {NomeProduto}", dto.Nome);
            return ResultadoDto<Guid>.RetornaErro("Não foi possível salvar o produto.");
        }
    }

    public async Task<ResultadoDto<bool>> AtualizarAsync(UpdateProdutoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Iniciando atualização do produto {ProdutoId}.", dto.IdProduto);

            var produtoExistente = await produtoRepository.ObterProdutosPorIdAsync(dto.IdProduto, cancellationToken);

            if (produtoExistente == null)
            {
                logger.LogWarning("Falha na atualização: Produto {ProdutoId} inexistente.", dto.IdProduto);
                return ResultadoDto<bool>.RetornaNaoEncontrado("Produto não encontrado para atualização.");
            }

            // Atualiza propriedades básicas
            if(!string.IsNullOrEmpty(dto.Nome))
                produtoExistente.Nome = dto.Nome;
            if(dto.PrecoCusto.HasValue)
                produtoExistente.PrecoCusto = dto.PrecoCusto.Value;
            if(dto.QuantidadeFilamento.HasValue)
                produtoExistente.QuantidadeFilamento = dto.QuantidadeFilamento.Value;
            if(dto.TempoImpressao.HasValue)
                produtoExistente.TempoImpressao = dto.TempoImpressao.Value;

            // Atualiza Composição (simplificado: remove e adiciona)
            produtoExistente.ComposicoesPai.Clear();
            if (dto.Componentes != null)
            {
                foreach (var comp in dto.Componentes)
                {
                    if (comp.IdProdutoFilho != Guid.Empty)
                    {
                        produtoExistente.ComposicoesPai.Add(new ProdutoComposicao { IdProdutoFilho = comp.IdProdutoFilho, Quantidade = comp.Quantidade });
                    }
                }
            }

            produtoRepository.Atualizar(produtoExistente);
            await produtoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Produto {ProdutoId} atualizado com sucesso.", dto.IdProduto);
            return ResultadoDto<bool>.RetornaSucesso("Produto atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar produto {ProdutoId}.", dto.IdProduto);
            return ResultadoDto<bool>.RetornaErro("Erro ao processar a atualização do produto.");
        }
    }
}
