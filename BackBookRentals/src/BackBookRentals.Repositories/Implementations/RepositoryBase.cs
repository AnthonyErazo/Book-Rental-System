using BackBookRentals.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BackBookRentals.Dto.Exception;
using BackBookRentals.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BackBookRentals.Repositories.Implementations;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class, IEntityBase
{
    protected readonly DbContext context;

    protected RepositoryBase(DbContext context)
    {
        this.context = context;
    }

    public virtual async Task<ICollection<TEntity>> GetAsync()
    {
        return await context.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy)
    {
        return await context.Set<TEntity>()
            .Where(predicate)
            .OrderBy(orderBy)
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(Guid id)
    {
        return await context.Set<TEntity>()
            .FindAsync(id);
    }

    public virtual async Task<Guid> AddAsync(TEntity entity)
    {
        await context.Set<TEntity>()
            .AddAsync(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync();
    }
}