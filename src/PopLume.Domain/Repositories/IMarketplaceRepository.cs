using PopLume.Domain.Entities;

namespace PopLume.Domain.Repositories;

public interface IMarketplaceRepository : IRepository<Marketplace>
{
    Task<IEnumerable<Marketplace>> ObterTodosMarketplacesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Marketplace>> ObterMarketplacesPorNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<IEnumerable<Marketplace>> ObterMarketplacesDesativadosAsync(CancellationToken cancellationToken = default);
    Task<Marketplace?> ObterMarketplacePorIdAsync(Guid idMarketplace, CancellationToken cancellationToken = default);
    Task<Marketplace?> ObterMarketplacePorIdParaAtualizacaoAsync(Guid idMarketplace, CancellationToken cancellationToken = default);
    Task<Marketplace?> ObterMarketplaceDesativadoPorIdAsync(Guid idMarketplace, CancellationToken cancellationToken = default);
}
