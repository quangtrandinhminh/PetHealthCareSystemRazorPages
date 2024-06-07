using System.Linq.Expressions;
using BusinessObject.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Repository.IBase;
using DataAccessLayer.Base;
namespace Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        public void Add(T entity) => BaseDAO<T>.Add(entity);

        public IQueryable<T> GetAll() => BaseDAO<T>.GetAll();

        public void Delete(T entity) => BaseDAO<T>.Delete(entity);

        public void Update(T entity) => BaseDAO<T>.Update(entity);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => BaseDAO<T>.FindByCondition(expression);
        
        public void AddRange(IEnumerable<T> entities) => BaseDAO<T>.AddRange(entities);
        
        public void RemoveRange(IEnumerable<T> entities) => BaseDAO<T>.RemoveRange(entities);
    }
}
