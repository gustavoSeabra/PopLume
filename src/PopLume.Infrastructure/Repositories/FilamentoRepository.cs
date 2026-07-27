using Microsoft.EntityFrameworkCore;
using PopLume.Domain.Entities;
using PopLume.Domain.Enums;
using PopLume.Domain.Repositories;
using PopLume.Infrastructure.DataProvider.Context;

namespace PopLume.Infrastructure.Repositories;

public class FilamentoRepository(PopLumeDbContext dbContext)
    : BaseRepository<Filamento>(dbContext), IFilamentoRepository
{
    public async Task<IEnumerable<Filamento>> ObterTodosFilamentosAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Set<Filamento>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<Filamento?> ObterFilamentoPorIdAsync(Guid idFilamento, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Filamento>()
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.IdFilamento == idFilamento, cancellationToken);

    public async Task<IEnumerable<Filamento>> ObterFilamentosPorCorAsync(string cor, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Filamento>()
            .Where(f => EF.Functions.ILike(f.Cor, CriarPadraoBusca(cor), LikeEscapeCharacter))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Filamento>> ObterFilamentosPorTipoAsync(TipoFilamento tipo, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Filamento>()
            .Where(f => f.Tipo == tipo)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}
