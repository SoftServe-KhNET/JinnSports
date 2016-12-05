using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JinnSports.DataAccessInterfaces
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        IList<T> GetAll(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where);
        T GetByID(int id);
        void Add(T item);
        void AddAll(T[] items);
        void Remove(T item);
    }
}
