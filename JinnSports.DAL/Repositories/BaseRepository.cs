using System;
using System.Collections.Generic;
using System.Linq;
using JinnSports.DAL.EF;
using JinnSports.DataAccessInterfaces;
using System.Data.Entity;

namespace JinnSports.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        SportsContext db; 
        internal DbSet<T> dbSet;

        public BaseRepository(SportsContext context)
        {
            this.db = context;
            this.dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public void Create(T entity)
        {
            dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            dbSet.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
        }

        //public IEnumerable<T> Find(Func<Result, Boolean> predicate)
        //{
        //    return db.Results.Where(predicate).ToList();
        //}

        public void Delete(int id)
        {
            T entityToDelete = dbSet.Find(id);
            db.Entry(entityToDelete).State = EntityState.Deleted;
        }
    }
}
