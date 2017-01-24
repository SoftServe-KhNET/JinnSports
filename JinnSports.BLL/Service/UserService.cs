using JinnSports.BLL.Dtos;
using JinnSports.BLL.Identity;
using JinnSports.BLL.Interfaces;
using JinnSports.DataAccessInterfaces.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JinnSports.BLL.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserDto, Guid> userManager;

        public UserService(IUnitOfWork unit)
        {
            this.userManager = new UserManager<UserDto, Guid>(new UserStore(unit));
        }

        public Task<UserDto> FindAsync(string userName, string password)
        {
            return this.userManager.FindAsync(userName, password);
        }

        public Task<UserDto> FindAsync(UserLoginInfo login)
        {
            return this.userManager.FindAsync(login);
        }

        public Task<IdentityResult> CreateAsync(UserDto user, string password)
        {
            return this.userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> CreateAsync(UserDto user)
        {
            return this.userManager.CreateAsync(user);
        }

        public Task<IdentityResult> RemoveLoginAsync(Guid userId, UserLoginInfo login)
        {
            return this.userManager.RemoveLoginAsync(userId, login);
        }

        public Task<IdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            return this.userManager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public Task<IdentityResult> AddPasswordAsync(Guid userId, string password)
        {
            return this.userManager.AddPasswordAsync(userId, password);
        }

        public Task<IdentityResult> AddLoginAsync(Guid userId, UserLoginInfo login)
        {
            return this.userManager.AddLoginAsync(userId, login);
        }

        public IList<UserLoginInfo> GetLogins(Guid userId)
        {
            return this.userManager.GetLogins(userId);
        }

        public Task<ClaimsIdentity> CreateIdentityAsync(UserDto user, string authenticationType)
        {
            return this.userManager.CreateIdentityAsync(user, authenticationType);
        }

        public UserDto FindById(Guid userId)
        {
            return this.userManager.FindById(userId);
        }

        public bool HasPassword(IPrincipal user)
        {
            var userDto = this.userManager.FindById(this.GetGuid(user.Identity.GetUserId()));
            if (user != null)
            {
                return userDto.PasswordHash != null;
            }
            return false;
        }

        public Guid GetGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }

        public void Dispose()
        {
            if (this.userManager != null)
            {
                this.userManager.Dispose(); 
            }
        }
    }
}
