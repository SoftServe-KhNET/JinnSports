using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using JinnSports.DataAccessInterfaces.Interfaces;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace JinnSports.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public BaseRepository(DbContext db)
        {
            this.DbSet = db.Set<T>();
            this.Context = db;
        }

        private DbSet<T> DbSet { get; }
        private DbContext Context { get; }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int skip = 0, 
            int take = -1)
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            query = orderBy?.Invoke(query) ?? query;

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            return query.ToList();
        }

        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual Task<T> GetByIdAsync(object id)
        {
            return this.DbSet.FindAsync(id);
        }

        public virtual int Count(Expression<Func<T, bool>> filter = null)
        {
            return filter == null
                ? this.DbSet.Count()
                : this.DbSet.Count(filter);
        }

        public virtual Task<int> CountAsync()
        {
            return this.DbSet.CountAsync();
        }

        public virtual void Insert(T entity)
        {
            this.DbSet.Add(entity);
        }

        public virtual void InsertAll(T[] entitiesToInsert)
        {
            this.DbSet.AddRange(entitiesToInsert);
        }

        public virtual void Update(T entityToUpdate)
        {            
            this.Context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;            
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = this.DbSet.Find(id);
            this.Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            this.Context.Entry(entityToDelete).State = System.Data.Entity.EntityState.Deleted;
        }
    }
}
