using Microsoft.EntityFrameworkCore;
using PopLume.Domain.Repositories;
using PopLume.Infrastructure.DataProvider.Context;

namespace PopLume.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected const string LikeEscapeCharacter = "\\";

    protected readonly PopLumeDbContext dbContext;

    public IUnitOfWork UnitOfWork => (IUnitOfWork)dbContext;

    protected BaseRepository(PopLumeDbContext dbContext) =>
        this.dbContext = dbContext;

    public void Adicionar(T entitidade) =>
        dbContext.Set<T>().Add(entitidade);


    public void Atualizar(T entitidade) =>
        dbContext.Set<T>().Update(entitidade);

    public virtual void Remover(T entitidade) =>
        dbContext.Set<T>().Remove(entitidade);

    public void DesanexaEntidade(T entitidade) =>
        dbContext.Entry(entitidade).State = EntityState.Detached;

    protected static string CriarPadraoBusca(string termo) =>
        $"%{termo.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_")}%";

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            dbContext.Dispose();
    }
}
