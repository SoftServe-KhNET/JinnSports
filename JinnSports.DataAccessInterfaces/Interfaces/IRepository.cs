using System;
using System.Collections.Generic;

namespace JinnSports.DataAccessInterfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        //IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Add(T item);
        void AddAll(T[] items);
        void Delete(T item);
    }
}
