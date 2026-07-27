using PopLume.Domain.Entities;
using PopLume.Domain.Enums;

namespace PopLume.Domain.Repositories;

public interface IFilamentoRepository : IRepository<Filamento>
{
    Task<IEnumerable<Filamento>> ObterTodosFilamentosAsync(CancellationToken cancellationToken = default);
    Task<Filamento?> ObterFilamentoPorIdAsync(Guid idFilamento, CancellationToken cancellationToken = default);
    Task<IEnumerable<Filamento>> ObterFilamentosPorCorAsync(string cor, CancellationToken cancellationToken = default);
    Task<IEnumerable<Filamento>> ObterFilamentosPorTipoAsync(TipoFilamento tipo, CancellationToken cancellationToken = default);
}
