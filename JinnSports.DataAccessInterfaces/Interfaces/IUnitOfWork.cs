using System;
using System.Threading.Tasks;

namespace JinnSports.DataAccessInterfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        void SaveChanges();

        Task SaveAsync();
    }
}
