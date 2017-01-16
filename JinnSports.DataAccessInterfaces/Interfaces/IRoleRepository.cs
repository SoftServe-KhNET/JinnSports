using JinnSports.Entities.Entities.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace JinnSports.DataAccessInterfaces.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role FindByName(string roleName);
        Task<Role> FindByNameAsync(string roleName);
        Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName);
    }
}
