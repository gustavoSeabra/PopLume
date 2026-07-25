namespace PopLume.Domain.Repositories;

public interface IRepository<T> : IDisposable where T : class
{
    IUnitOfWork UnitOfWork { get; }

    void Adicionar(T entitidade);
    void Atualizar(T entitidade);
    void Remover(T entitidade);
    void DesanexaEntidade(T entitidade);
}