using System.Linq.Expressions;
using BackBookRentals.Entities;

namespace BackBookRentals.Repositories.Abstractions;

public interface IRepositoryBase<TEntity> where TEntity : class,IEntityBase
{
    Task<ICollection<TEntity>> GetAsync();
    Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<ICollection<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy);
    Task<TEntity?> GetAsync(Guid id);
    Task<Guid> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}