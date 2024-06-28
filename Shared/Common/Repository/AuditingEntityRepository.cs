using DreamWeddingApi.Shared.Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace DreamWeddingApi.Shared.Common.Repository;

public class AuditingEntityRepository<TEntity>(DbContext dbContext)
    where TEntity : AuditingEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public virtual IEnumerable<TEntity> FindAll()
    {
        return _dbSet.ToList();
    }

    public virtual TEntity? FindById(long id)
    {
        return _dbSet.Find(id);
    }

    public virtual TEntity Add(TEntity entity, bool saveChanges = false)
    {
        _dbSet.Add(entity);

        if (saveChanges)
        {
            dbContext.SaveChanges();
        }

        return entity;
    }

    public virtual void Update(TEntity entity, bool saveChanges = false)
    {
        _dbSet.Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;

        if (saveChanges)
        {
            dbContext.SaveChanges();
        }
    }

    public virtual void Delete(long id)
    {
        var entity = _dbSet.Find(id);

        if (entity is null)
        {
            return;
        }

        Delete(entity);
    }

    public virtual void Delete(TEntity entity, bool saveChanges = false)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);

        if (saveChanges)
        {
            dbContext.SaveChanges();
        }
    }
}