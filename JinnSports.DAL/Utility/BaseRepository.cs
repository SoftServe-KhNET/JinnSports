using System;
using System.Collections.Generic;
using System.Linq;
using JinnSports.DAL.EFContext;
using System.Data.Entity;
using JinnSports.Interfaces.DALInterfaces;
using System.Linq.Expressions;
using JinnSports.DAL.Utility;

namespace JinnSports.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private EFUnitOfWork dbContext;
        private DbSet<T> dbSet;

        public BaseRepository()
        {
            dbContext = new EFUnitOfWork();
            dbSet = dbContext.Set<T>();
        }

        public BaseRepository(IUnitOfWork UnitOfWork)
        {
            dbContext = (EFUnitOfWork)UnitOfWork;
            dbSet = dbContext.Set<T>();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IList<T> GetAll()
        {
            return dbSet.ToList();
        }

        public IList<T> GetWithCondition(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
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

        public void Remove(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where(where).AsEnumerable();
            foreach (T obj in objects)
            {
                dbSet.Remove(obj);
            }
        }
    }
}
