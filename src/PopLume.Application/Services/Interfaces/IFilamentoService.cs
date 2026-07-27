using PopLume.Application.Dtos;
using PopLume.Domain.Enums;

namespace PopLume.Application.Services.Interfaces;

public interface IFilamentoService
{
    Task<ResultadoDto<IEnumerable<FilamentoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<ResultadoDto<FilamentoDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<FilamentoDto>>> ObterPorCorAsync(string cor, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<FilamentoDto>>> ObterPorTipoAsync(TipoFilamento tipo, CancellationToken cancellationToken = default);
    Task<ResultadoDto<Guid>> AdicionarAsync(CreateFilamentoDto dto, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> AtualizarAsync(UpdateFilamentoDto dto, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
