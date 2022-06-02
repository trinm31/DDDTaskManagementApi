using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Entities { get; }
        DbContext DbContext { get; }
        /// <summary>
        /// Get all items of an entity by asynchronous method
        /// </summary>
        /// <returns></returns>
        Task<IList<T>> GetAllAsync();
        /// <summary>
        /// Fin one item of an entity synchronous method
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        T Find(params object[] keyValues);
        /// <summary>
        /// Find one item of an entity by asynchronous method 
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<T> FindAsync(params object[] keyValues);
        /// <summary>
        /// Insert item into an entity by asynchronous method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task InsertAsync(T entity, bool saveChanges = true);
        /// <summary>
        /// Insert multiple items into an entity by asynchronous method
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task InsertRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
        /// <summary>
        /// Remove one item from an entity by asynchronous method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task DeleteAsync(int id, bool saveChanges = true);
        /// <summary>
        /// Remove one item from an entity by asynchronous method
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity, bool saveChanges = true);
        /// <summary>
        /// Remove multiple items from an entity by asynchronous method
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
        /// <summary>
        /// To Get All entity with conditions
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
        );
        /// <summary>
        /// To get an entity with conditions
        /// </summary>
        /// <param name="filter"></param>  
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
        );
    }
}
