using Microsoft.AspNetCore.Mvc;
using PopLume.Application.Dtos;
using PopLume.Application.Services.Interfaces;
using PopLume.Domain.Enums;

namespace PopLume.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilamentoController(IFilamentoService filamentoService) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<FilamentoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var resultado = await filamentoService.ObterTodosAsync(cancellationToken);
        return Responder(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResultadoDto<FilamentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<FilamentoDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await filamentoService.ObterPorIdAsync(id, cancellationToken);
        return Responder(resultado);
    }

    [HttpGet("buscar/cor/{cor}")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<FilamentoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorCor(string cor, CancellationToken cancellationToken)
    {
        var resultado = await filamentoService.ObterPorCorAsync(cor, cancellationToken);
        return Responder(resultado);
    }

    [HttpGet("buscar/tipo/{tipo}")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<FilamentoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorTipo(TipoFilamento tipo, CancellationToken cancellationToken)
    {
        var resultado = await filamentoService.ObterPorTipoAsync(tipo, cancellationToken);
        return Responder(resultado);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Adicionar([FromBody] CreateFilamentoDto dto, CancellationToken cancellationToken)
    {
        var resultado = await filamentoService.AdicionarAsync(dto, cancellationToken);
        return resultado.Ok
            ? CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado)
            : BadRequest(resultado);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualizar([FromBody] UpdateFilamentoDto dto, CancellationToken cancellationToken)
    {
        var resultado = await filamentoService.AtualizarAsync(dto, cancellationToken);
        return Responder(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await filamentoService.RemoverAsync(id, cancellationToken);
        return Responder(resultado);
    }
}
