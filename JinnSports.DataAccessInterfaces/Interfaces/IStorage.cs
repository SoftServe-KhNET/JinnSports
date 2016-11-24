using System;
using System.Collections.Generic;

namespace JinnSports.DataAccessInterfaces
{
    public interface IStorage<T> : IDisposable where T : class
    {
        IRepository<T> GetRepository();
        void SaveChanges();
    }
}
