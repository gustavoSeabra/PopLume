using Microsoft.EntityFrameworkCore;
using PopLume.Domain.Entities;
using PopLume.Domain.Repositories;
using PopLume.Infrastructure.DataProvider.Context;

namespace PopLume.Infrastructure.Repositories;

public class EquipamentoRepository : BaseRepository<Equipamento>, IEquipamentoRepository
{
    public EquipamentoRepository(PopLumeDbContext dbContext) : base(dbContext)
    {
    }

    public override void Remover(Equipamento equipamento)
    {
        equipamento.Excluir();
        dbContext.Set<Equipamento>().Update(equipamento);
    }

    public async Task<IEnumerable<Equipamento>> ObterEquipamentosPorApelidoAsync(string apelido, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Equipamento>()
            .Where(e => EF.Functions.ILike(e.Apelido, CriarPadraoBusca(apelido), LikeEscapeCharacter))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    

    public async Task<Equipamento?> ObterEquipamentosPorIdAsync(Guid idEquipamento, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Equipamento>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEquipamento == idEquipamento, cancellationToken);

    public async Task<IEnumerable<Equipamento>> ObterEquipamentosDesativadosAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Set<Equipamento>()
            .IgnoreQueryFilters()
            .Where(e => e.Excluido)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<Equipamento?> ObterEquipamentoDesativadoPorIdAsync(Guid idEquipamento, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Equipamento>()
            .IgnoreQueryFilters()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEquipamento == idEquipamento && e.Excluido, cancellationToken);

    public async Task<IEnumerable<Equipamento>> ObterEquipamentosPorNomeAsync(string nome, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Equipamento>()
            .Where(e => EF.Functions.ILike(e.Nome, CriarPadraoBusca(nome), LikeEscapeCharacter))
            .AsNoTracking()
            .ToListAsync(cancellationToken);


    public async Task<IEnumerable<Equipamento>> ObterTodosEquipamentosAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Set<Equipamento>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}
