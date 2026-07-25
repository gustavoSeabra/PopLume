using PopLume.Application.Dtos;

namespace PopLume.Application.Services.Interfaces;

public interface IProdutoService
{
    Task<ResultadoDto<IEnumerable<ProdutoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<ResultadoDto<ProdutoDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<ProdutoDto>>> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<ResultadoDto<Guid>> AdicionarAsync(CreateProdutoDto dto, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> AtualizarAsync(UpdateProdutoDto dto, CancellationToken cancellationToken = default);
}
