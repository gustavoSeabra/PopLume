using PopLume.Domain.Entities;

namespace PopLume.Domain.Repositories;

public interface IEquipamentoRepository : IRepository<Equipamento>
{
    Task<IEnumerable<Equipamento>> ObterTodosEquipamentosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Equipamento>> ObterEquipamentosPorNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<IEnumerable<Equipamento>> ObterEquipamentosPorApelidoAsync(string apelido, CancellationToken cancellationToken = default);
    Task<IEnumerable<Equipamento>> ObterEquipamentosDesativadosAsync(CancellationToken cancellationToken = default);
    Task<Equipamento?> ObterEquipamentoDesativadoPorIdAsync(Guid idEquipamento, CancellationToken cancellationToken = default);
    Task<Equipamento?> ObterEquipamentosPorIdAsync(Guid idEquipamento, CancellationToken cancellationToken = default);
}
