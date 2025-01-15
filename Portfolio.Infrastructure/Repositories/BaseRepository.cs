using Portfolio.Domain.Entities;

namespace Portfolio.Infrastructure.Repositories;

public abstract class BaseRepository<T>(PortfolioDbContext dbContext)
    where T : EntityBaseWithInt
{
    protected readonly PortfolioDbContext dbContext = dbContext;

    public virtual T? GetById(int id)
    {
        return dbContext.Set<T>().Find(id);
    }
    
    public virtual T Create(T entity)
    {
        dbContext.Set<T>().Add(entity);
        dbContext.SaveChanges();
        return entity;
    }
    
    public virtual T Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        dbContext.SaveChanges();
        return entity;
    }
    
    public virtual void Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        dbContext.SaveChanges();
    }
    
    public virtual void RemoveRows(List<int> ids)
    {
        var rowsToRemove = dbContext.Set<T>()
            .Where(x => ids.Contains(x.Id));

        dbContext.Set<T>().RemoveRange(rowsToRemove);
        dbContext.SaveChanges();
    }
}
