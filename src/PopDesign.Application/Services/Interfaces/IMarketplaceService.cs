using PopDesign.Application.Dtos;

namespace PopDesign.Application.Services.Interfaces;

public interface IMarketplaceService
{
    Task<ResultadoDto<IEnumerable<MarketplaceDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<ResultadoDto<MarketplaceDto?>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<MarketplaceDto>>> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<ResultadoDto<IEnumerable<MarketplaceDto>>> ObterDesativadosAsync(CancellationToken cancellationToken = default);
    Task<ResultadoDto<Guid>> AdicionarAsync(CreateMarketplaceDto dto, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> AtualizarAsync(UpdateMarketplaceDto dto, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> DesativarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultadoDto<bool>> RestaurarAsync(Guid id, CancellationToken cancellationToken = default);
}
