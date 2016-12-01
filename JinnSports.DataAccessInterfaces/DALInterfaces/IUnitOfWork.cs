using System;

namespace JinnSports.Interfaces.DALInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
