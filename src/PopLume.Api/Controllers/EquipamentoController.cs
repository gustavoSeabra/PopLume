using Microsoft.AspNetCore.Mvc;
using PopLume.Application.Dtos;
using PopLume.Application.Services.Interfaces;

namespace PopLume.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipamentoController(IEquipamentoService equipamentoService) : BaseController
{
    /// <summary>
    /// Obtém a listagem completa de equipamentos ativos.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<EquipamentoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.ObterTodosAsync(cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Recupera um equipamento através do seu identificador único (GUID).
    /// </summary>
    /// <param name="id">ID do equipamento.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResultadoDto<EquipamentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<EquipamentoDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.ObterPorIdAsync(id, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Filtra equipamentos baseando-se em uma parte do nome.
    /// </summary>
    /// <param name="nome">Termo de busca para o nome.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("buscar/nome/{nome}")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<EquipamentoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorNome(string nome, CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.ObterPorNomeAsync(nome, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Filtra equipamentos baseando-se em uma parte do apelido.
    /// </summary>
    /// <param name="apelido">Termo de busca para o apelido.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("buscar/apelido/{apelido}")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<EquipamentoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorApelido(string apelido, CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.ObterPorApelidoAsync(apelido, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Obtém a listagem completa de equipamentos desativados.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("desativados")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<EquipamentoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterDesativados(CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.ObterDesativadosAsync(cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Cria um novo registro de equipamento no sistema.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação do equipamento.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Adicionar([FromBody] CreateEquipamentoDto dto, CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.AdicionarAsync(dto, cancellationToken);

        return resultado.Ok
            ? CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado)
            : BadRequest(resultado);
    }

    /// <summary>
    /// Atualiza os dados de um equipamento existente.
    /// </summary>
    /// <param name="dto">Dados atualizados do equipamento.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPut]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualizar([FromBody] UpdateEquipamentoDto dto, CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.AtualizarAsync(dto, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Desativa um equipamento sem removê-lo fisicamente do banco.
    /// </summary>
    /// <param name="id">ID do equipamento.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.DesativarAsync(id, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Restaura um equipamento desativado.
    /// </summary>
    /// <param name="id">ID do equipamento.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPatch("{id:guid}/restaurar")]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Restaurar(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await equipamentoService.RestaurarAsync(id, cancellationToken);
        return Responder(resultado);
    }
}
