using Microsoft.AspNetCore.Mvc;
using PopDesign.Application.Dtos;
using PopDesign.Application.Services.Interfaces;

namespace PopDesign.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketplaceController(IMarketplaceService marketplaceService) : BaseController
{
    /// <summary>
    /// Obtém a listagem completa de marketplaces ativos.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<MarketplaceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.ObterTodosAsync(cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Recupera um marketplace através do seu identificador único (GUID).
    /// </summary>
    /// <param name="id">ID do marketplace.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResultadoDto<MarketplaceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<MarketplaceDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.ObterPorIdAsync(id, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Filtra marketplaces baseando-se em uma parte do nome.
    /// </summary>
    /// <param name="nome">Termo de busca para o nome.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("buscar/nome/{nome}")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<MarketplaceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorNome(string nome, CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.ObterPorNomeAsync(nome, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Obtém a listagem completa de marketplaces desativados.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("desativados")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<MarketplaceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterDesativados(CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.ObterDesativadosAsync(cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Cria um novo registro de marketplace no sistema.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação do marketplace.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Adicionar([FromBody] CreateMarketplaceDto dto, CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.AdicionarAsync(dto, cancellationToken);

        return resultado.Ok
            ? CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado)
            : BadRequest(resultado);
    }

    /// <summary>
    /// Atualiza os dados de um marketplace existente e sincroniza suas taxas.
    /// </summary>
    /// <param name="dto">Dados atualizados do marketplace.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPut]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualizar([FromBody] UpdateMarketplaceDto dto, CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.AtualizarAsync(dto, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Desativa um marketplace sem removê-lo fisicamente do banco.
    /// </summary>
    /// <param name="id">ID do marketplace.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.DesativarAsync(id, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Restaura um marketplace desativado.
    /// </summary>
    /// <param name="id">ID do marketplace.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPatch("{id:guid}/restaurar")]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Restaurar(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await marketplaceService.RestaurarAsync(id, cancellationToken);
        return Responder(resultado);
    }
}
