using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JinnSports.Interfaces.DALInterfaces

{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IList<T> GetAll();
        IList<T> GetWithCondition(Expression<Func<T, bool>> where);
        void Add(T item);
        void AddAll(T[] items);
        void Remove(T item);
        void Remove(Expression<Func<T, bool>> where);
    }
}
