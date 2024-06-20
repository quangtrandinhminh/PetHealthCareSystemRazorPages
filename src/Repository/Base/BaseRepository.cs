using System.Linq.Expressions;
using BusinessObject.Entities.Base;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Base;
namespace Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        public IQueryable<T?> GetAll() => BaseDao<T>.GetAll();
        public async Task<IList<T>?> GetAllAsync() => await BaseDao<T>.GetAllAsync();
        public IQueryable<T> GetAllWithCondition(Expression<Func<T, bool>> predicate = null,
             params Expression<Func<T, object>>[] includeProperties) 
            => BaseDao<T>.GetAllWithCondition(predicate, includeProperties);
        public T? GetById(int id) => BaseDao<T>.GetById(id);
        public async Task<T?> GetByIdAsync(int id) => await BaseDao<T>.GetByIdAsync(id);
        public Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate,
            bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        => BaseDao<T>.GetSingleAsync(predicate, isIncludeDeleted, includeProperties);

        public void Add(T entity) => BaseDao<T>.Add(entity);
        public async Task<T> AddAsync(T entity) => await BaseDao<T>.AddAsync(entity);
        public void AddRange(IEnumerable<T> entities) => BaseDao<T>.AddRange(entities);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await BaseDao<T>.AddRangeAsync(entities);
        public void Update(T entity) => BaseDao<T>.Update(entity);
        public async Task UpdateAsync(T entity) => await BaseDao<T>.UpdateAsync(entity);
        public async Task UpdateRangeAsync(IEnumerable<T> entities) => await BaseDao<T>.UpdateRangeAsync(entities);
        public void Delete(T entity) => BaseDao<T>.Delete(entity);
        public async Task DeleteAsync(T entity) => await BaseDao<T>.DeleteAsync(entity);
        public void RemoveRange(IEnumerable<T> entities) => BaseDao<T>.RemoveRange(entities);
        public async Task RemoveRangeAsync(IEnumerable<T> entities) => await BaseDao<T>.RemoveRangeAsync(entities);
        public IQueryable<T?> FindByCondition(Expression<Func<T?, bool>> expression) => BaseDao<T>.FindByCondition(expression);
        public async Task<IList<T?>> FindByConditionAsync(Expression<Func<T?, bool>> expression) => await BaseDao<T>.FindByConditionAsync(expression);
    }
}
