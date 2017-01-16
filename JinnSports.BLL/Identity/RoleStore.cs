using JinnSports.BLL.Dtos;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace JinnSports.BLL.Identity
{
    public class RoleStore : IRoleStore<RoleDto, Guid>, IQueryableRoleStore<RoleDto, Guid>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;

        public RoleStore(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #region IQueryableRoleStore<IdentityRole, Guid> Members
        public IQueryable<RoleDto> Roles
        {
            get
            {
                return this.unitOfWork.GetRepository<Role>().Get()
                    .Select(x => new RoleDto { Id = x.RoleId, Name = x.Name })
                    .AsQueryable<RoleDto>();
            }
        }
        #endregion

        #region IRoleStore<IdentityRole, Guid> Members
        public Task CreateAsync(RoleDto role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
                
            Role r = Mapper.Map<RoleDto, Role>(role);

            this.unitOfWork.GetRepository<Role>().Insert(r);
            return this.unitOfWork.SaveAsync();
        }

        public Task DeleteAsync(RoleDto role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
                
            Role r = Mapper.Map<RoleDto, Role>(role);

            this.unitOfWork.GetRepository<Role>().Delete(r);
            return this.unitOfWork.SaveAsync();
        }

        public Task<RoleDto> FindByIdAsync(Guid roleId)
        {
            Role role = this.unitOfWork.GetRepository<Role>().Get(r => r.RoleId == roleId).FirstOrDefault();
            return Task.FromResult<RoleDto>(Mapper.Map<Role, RoleDto>(role));
        }

        public Task<RoleDto> FindByNameAsync(string roleName)
        {
            Role role = this.unitOfWork.GetRepository<Role>().Get(r => r.Name == roleName).FirstOrDefault();
            return Task.FromResult<RoleDto>(Mapper.Map<Role, RoleDto>(role));
        }

        public Task UpdateAsync(RoleDto role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }                
            Role r = Mapper.Map<RoleDto, Role>(role);
            this.unitOfWork.GetRepository<Role>().Update(r);
            return this.unitOfWork.SaveAsync();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }
        #endregion        
    }
}
