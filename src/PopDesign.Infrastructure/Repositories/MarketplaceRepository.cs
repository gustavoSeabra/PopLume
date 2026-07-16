using Microsoft.EntityFrameworkCore;
using PopDesign.Domain.Entities;
using PopDesign.Domain.Repositories;
using PopDesign.Infrastructure.DataProvider.Context;

namespace PopDesign.Infrastructure.Repositories;

public class MarketplaceRepository : BaseRepository<Marketplace>, IMarketplaceRepository
{
    public MarketplaceRepository(PopDesignDbContext dbContext) : base(dbContext)
    {
    }

    public override void Remover(Marketplace marketplace)
    {
        marketplace.Excluir();
        dbContext.Set<Marketplace>().Update(marketplace);
    }

    public async Task<Marketplace?> ObterMarketplacePorIdAsync(Guid idMarketplace, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Marketplace>()
            .Include(m => m.TaxasMarketplace)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdMarketplace == idMarketplace, cancellationToken);

    public async Task<Marketplace?> ObterMarketplacePorIdParaAtualizacaoAsync(Guid idMarketplace, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Marketplace>()
            .AsTracking()
            .Include(m => m.TaxasMarketplace)
            .FirstOrDefaultAsync(m => m.IdMarketplace == idMarketplace, cancellationToken);

    public async Task<Marketplace?> ObterMarketplaceDesativadoPorIdAsync(Guid idMarketplace, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Marketplace>()
            .IgnoreQueryFilters()
            .Include(m => m.TaxasMarketplace)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdMarketplace == idMarketplace && m.Excluido, cancellationToken);

    public async Task<IEnumerable<Marketplace>> ObterMarketplacesPorNomeAsync(string nome, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Marketplace>()
            .Where(m => EF.Functions.ILike(m.Nome, CriarPadraoBusca(nome), LikeEscapeCharacter))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Marketplace>> ObterMarketplacesDesativadosAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Set<Marketplace>()
            .IgnoreQueryFilters()
            .Where(m => m.Excluido)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Marketplace>> ObterTodosMarketplacesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Set<Marketplace>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}
