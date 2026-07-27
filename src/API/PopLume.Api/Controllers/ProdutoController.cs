using Microsoft.AspNetCore.Mvc;
using PopLume.Application.Dtos;
using PopLume.Application.Services.Interfaces;

namespace PopLume.Api.Controllers;
    
[ApiController]
[Route("api/[controller]")]
public class ProdutoController(IProdutoService produtoService) : BaseController
{
    /// <summary>
    /// Obtém a listagem completa de produtos.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<ProdutoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var resultado = await produtoService.ObterTodosAsync(cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Recupera um produto através do seu identificador único (GUID).
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResultadoDto<ProdutoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<ProdutoDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await produtoService.ObterPorIdAsync(id, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Filtra produtos baseando-se em uma parte do nome.
    /// </summary>
    /// <param name="nome">Termo de busca para o nome.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpGet("buscar/{nome}")]
    [ProducesResponseType(typeof(ResultadoDto<IEnumerable<ProdutoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorNome(string nome, CancellationToken cancellationToken)
    {
        var resultado = await produtoService.ObterPorNomeAsync(nome, cancellationToken);
        return Responder(resultado);
    }

    /// <summary>
    /// Cria um novo registro de produto no sistema.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultadoDto<Guid>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Adicionar([FromBody] CreateProdutoDto dto, CancellationToken cancellationToken)
    {
        var resultado = await produtoService.AdicionarAsync(dto, cancellationToken);

        return resultado.Ok 
            ? CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado) 
            : BadRequest(resultado);
    }

    /// <summary>
    /// Atualiza os dados de um produto existente.
    /// </summary>
    /// <param name="dto">Dados atualizados do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição.</param>
    [HttpPut]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResultadoDto<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualizar([FromBody] UpdateProdutoDto dto, CancellationToken cancellationToken)
    {
        var resultado = await produtoService.AtualizarAsync(dto, cancellationToken);
        return Responder(resultado);
    }
}
