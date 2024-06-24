using System.Linq.Expressions;

namespace Repository.Base;

public interface IBaseRepository<T> where T : class, new()
{
    IQueryable<T?> GetAll();
    Task<IList<T>?> GetAllAsync();
    IQueryable<T> GetAllWithCondition(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
    T? GetById(int id);
    Task<T?> GetByIdAsync(int id);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties);
    void Add(T? entity);
    Task<T> AddAsync(T entity);
    void AddRange(IEnumerable<T?> entities);
    Task AddRangeAsync(IEnumerable<T?> entities);
    void Update(T entity);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T?> entities);
    void Delete(T? entity);
    Task DeleteAsync(T entity);
    void RemoveRange(IEnumerable<T?> entities);
    Task RemoveRangeAsync(IEnumerable<T?> entities);
    IQueryable<T?> FindByCondition(Expression<Func<T?, bool>> expression);
    Task<IList<T?>> FindByConditionAsync(Expression<Func<T?, bool>> expression);
}
