using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Repository.IBase;

    public interface IBaseRepository<T> where T : class, new()
    {
        void Add(T entity);
        IQueryable<T> GetAll();
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    }
