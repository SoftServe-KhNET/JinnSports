using System;
using System.Collections.Generic;

namespace JinnSports.DataAccessInterfaces
{
    public interface IUnitOfWork<T> : IDisposable where T : class
    {
        IRepository<T> GetRepository();
        void Save();
    }
}
