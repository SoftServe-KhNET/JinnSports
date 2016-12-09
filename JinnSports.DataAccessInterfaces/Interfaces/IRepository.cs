using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JinnSports.DataAccessInterfaces.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where);
        T GetById(int id);
        void Add(T item);
        void AddAll(T[] items);
        void Remove(T item);
    }
}
