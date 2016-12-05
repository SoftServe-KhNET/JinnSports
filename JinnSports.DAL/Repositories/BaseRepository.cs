using System;
using System.Collections.Generic;
using System.Linq;
using JinnSports.DAL.EFContext;
using JinnSports.DataAccessInterfaces;
using System.Data.Entity;
using System.Linq.Expressions;

namespace JinnSports.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    { 
        private DbSet<T> dbSet;

        public BaseRepository(SportsContext db)
        {
            dbSet = db.Set<T>();
        }        

        public IList<T> GetAll()
        {
            return dbSet.ToList();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }

        public T GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public void Add(T item)
        {
            dbSet.Add(item);
        }

        public void AddAll(T[] items)
        {
            dbSet.AddRange(items);
        }

        public void Remove(T item)
        {
            dbSet.Remove(item);
        }
    }
}
