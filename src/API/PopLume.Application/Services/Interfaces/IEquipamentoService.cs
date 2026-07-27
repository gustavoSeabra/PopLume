using PopLume.Application.Dtos;

namespace PopLume.Application.Services.Interfaces;

public interface IEquipamentoService
{
    Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<ResultadoDto<EquipamentoDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterPorApelidoAsync(string apelido, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<EquipamentoDto>>> ObterDesativadosAsync(CancellationToken cancellationToken = default);
    Task<ResultadoDto<Guid>> AdicionarAsync(CreateEquipamentoDto dto, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> AtualizarAsync(UpdateEquipamentoDto dto, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> DesativarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> RestaurarAsync(Guid id, CancellationToken cancellationToken = default);
}
