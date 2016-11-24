using System;
using System.Collections.Generic;
using System.Linq;
using JinnSports.DAL.Context;
using JinnSports.DataAccessInterfaces;
using System.Data.Entity;

namespace JinnSports.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> dbSet;

        public BaseRepository(DbSet<T> dbSet)
        {
            this.dbSet = dbSet;
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

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(T[] entitys)
        {
            dbSet.AddRange(entitys);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
