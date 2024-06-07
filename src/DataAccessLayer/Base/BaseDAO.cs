using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BusinessObject.Entities.Base;
using Repository;

namespace DataAccessLayer.Base
{
    public class BaseDAO<T> where T : BaseEntity, new()
    {
        protected static readonly AppDbContext _context = new AppDbContext();
        private static DbSet<T> _dbSet;

        public static IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public static T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public static async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public static T Add(T entity)
        {
            entity.DeletedTime = null;
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public static async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public static void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public static void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking();
        }

        public static AppDbContext GetDbContext()
        {
            return _context;
        }
        
        public static List<T> AddRange(IEnumerable<T> entities)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            List<T> list = new List<T>();
            foreach (T val in entities)
            {
                val.CreatedTime = utcNow;
                T item = Add(val);
                list.Add(item);
            }

            _context.SaveChanges();
            return list;
        }
        
        public static void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}