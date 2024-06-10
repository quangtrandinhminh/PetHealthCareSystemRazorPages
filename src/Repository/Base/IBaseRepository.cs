using System.Linq.Expressions;

namespace Repository.Base;

    public interface IBaseRepository<T> where T : class, new()
    {
        IQueryable<T?> GetAll();
        Task<IList<T>?> GetAllAsync();
        T? GetById(int id);
        Task<T?> GetByIdAsync(int id);
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
