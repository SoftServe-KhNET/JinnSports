using System;
using System.Threading.Tasks;

namespace JinnSports.DataAccessInterfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IExternalLoginRepository ExternalLoginRepository { get; }
        //IRoleRepository RoleRepository { get; }
        //IUserRepository UserRepository { get; }

        IRepository<T> GetRepository<T>() where T : class;

        void SaveChanges();

        Task SaveAsync();
    }
}
