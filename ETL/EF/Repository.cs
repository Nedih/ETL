using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ETL.EF
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext _context;
        private DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }
        public IQueryable<TEntity> Where(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }
        public int Count()
        {
            return _dbSet.Count();
        }
        public int Count(Func<TEntity, bool> predicate)
        {
            return _dbSet.Count(predicate);
        }
        public void AddAndSave(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error adding entity: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        public void BulkAddAndSave(IEnumerable<TEntity> entities)
        {
            try
            {
                _context.BulkInsertOrUpdate(entities);
                _context.BulkSaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Bulk insert failed: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
        public void RemoveAndSave(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error removing entity: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public void BulkRemoveAndSave(IEnumerable<TEntity> entities)
        {
            try
            {
                _context.BulkDelete(entities);
                _context.BulkSaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Bulk delete failed: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }

        public void UpdateAndSave(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error updating entity: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public void BulkUpdateAndSave(IEnumerable<TEntity> entities)
        {
            try
            {
                _context.BulkUpdate(entities);
                _context.BulkSaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Bulk update failed: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Async save failed: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
