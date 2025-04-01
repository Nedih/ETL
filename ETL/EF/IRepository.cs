using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETL.EF
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void AddAndSave(TEntity entity);
        void BulkAddAndSave(IEnumerable<TEntity> entities);
        void UpdateAndSave(TEntity entity);
        void BulkUpdateAndSave(IEnumerable<TEntity> entities);
        void RemoveAndSave(TEntity entity);
        void BulkRemoveAndSave(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Where(Func<TEntity, bool> predicate);
        TEntity FirstOrDefault(Func<TEntity, bool> predicate);
        int Count();
        int Count(Func<TEntity, bool> predicate);
        Task SaveAsync();
        
    }
}
