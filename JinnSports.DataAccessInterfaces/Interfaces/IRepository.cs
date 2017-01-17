using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JinnSports.DataAccessInterfaces.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int skip = 0, 
            int take = -1);
        
        T GetById(object id);

        Task<T> GetByIdAsync(object id);

        int Count(Expression<Func<T, bool>> filter = null);

        Task<int> CountAsync();

        void Insert(T entity);

        void InsertAll(T[] entitiesToInsert);

        void Update(T entity);

        void Delete(object id);

        void Delete(T entityToDelete);
    }
}
