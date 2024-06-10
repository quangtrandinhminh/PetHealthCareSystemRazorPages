using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BusinessObject.Entities.Base;

namespace DataAccessLayer.Base
{
    public class BaseDao<T> where T : BaseEntity, new()
    {
        private static readonly AppDbContext _context = new ();
        private static DbSet<T> _dbSet;

        public static IQueryable<T?> GetAll()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public static async Task<List<T?>> GetAllAsync()
        {
            return await _dbSet.AsQueryable().AsNoTracking().ToListAsync();
        }

        public static T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public static async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public static T Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public static async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public static void AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            _context.SaveChanges();
        }

        public static async Task AddRangeAsync(IEnumerable<T?> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public static void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static async Task UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public static async Task UpdateRangeAsync(IEnumerable<T?> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public static void Delete(T? entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public static async Task DeleteAsync(T? entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public static void RemoveRange(IEnumerable<T?> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }

        public static async Task RemoveRangeAsync(IEnumerable<T?> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public static IQueryable<T?> FindByCondition(Expression<Func<T?, bool>> expression)
        {
            return _dbSet.Where(expression).AsQueryable().AsNoTracking();
        }

        public static async Task<IList<T?>> FindByConditionAsync(Expression<Func<T?, bool>> expression)
        {
            return await _dbSet.Where(expression).AsQueryable().AsNoTracking().ToListAsync();
        }
    }
}