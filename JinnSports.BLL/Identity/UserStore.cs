using JinnSports.BLL.Dtos;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace JinnSports.BLL.Identity
{
    public class UserStore : IUserLoginStore<UserDto, Guid>, IUserClaimStore<UserDto, Guid>, IUserRoleStore<UserDto, Guid>, IUserPasswordStore<UserDto, Guid>, IUserSecurityStampStore<UserDto, Guid>, IUserStore<UserDto, Guid>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;

        public UserStore(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #region IUserStore<IdentityUser, Guid> Members
        public Task CreateAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            User u = Mapper.Map<UserDto, User>(user);

            this.unitOfWork.GetRepository<User>().Insert(u);
            return this.unitOfWork.SaveAsync();
        }

        public Task DeleteAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }                

            User u = Mapper.Map<UserDto, User>(user);

            this.unitOfWork.GetRepository<User>().Delete(u);
            return this.unitOfWork.SaveAsync();
        }

        public Task<UserDto> FindByIdAsync(Guid userId)
        {
            User user = this.unitOfWork.GetRepository<User>().Get(u => u.UserId == userId).FirstOrDefault();
            return Task.FromResult<UserDto>(Mapper.Map<User, UserDto>(user));
        }

        public Task<UserDto> FindByNameAsync(string userName)
        {
            User user = this.unitOfWork.GetRepository<User>().Get(u => u.UserName == userName).FirstOrDefault();
            return Task.FromResult<UserDto>(Mapper.Map<User, UserDto>(user));
        }

        public Task UpdateAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentException("user");
            }                

            User user = this.unitOfWork.GetRepository<User>().Get((System.Linq.Expressions.Expression<Func<User, bool>>)(us => us.UserId == userDto.Id)).FirstOrDefault();
            if (user == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            user = Mapper.Map<UserDto, User>((UserDto)userDto);
            
            //unitOfWork.GetRepository<User>().Update(u);
            return this.unitOfWork.SaveAsync();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }
        #endregion

        #region IUserClaimStore<IdentityUser, Guid> Members
        public Task AddClaimAsync(UserDto user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            var c = new Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);

            this.unitOfWork.GetRepository<User>().Update(u);
            return this.unitOfWork.SaveAsync();
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            return Task.FromResult<IList<System.Security.Claims.Claim>>(u.Claims.Select(x => new System.Security.Claims.Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(UserDto user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            Claim c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            this.unitOfWork.GetRepository<User>().Update(u);
            return this.unitOfWork.SaveAsync();
        }
        #endregion

        #region IUserLoginStore<IdentityUser, Guid> Members
        public Task AddLoginAsync(UserDto user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            var l = new ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = u
            };
            u.Logins.Add(l);

            //unitOfWork.GetRepository<User>().Update(u);
            return this.unitOfWork.SaveAsync();
        }

        public Task<UserDto> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }                

            UserDto userDto = default(UserDto);

            ExternalLogin l = this.unitOfWork.GetRepository<ExternalLogin>()
                .Get(log => (log.ProviderKey == login.ProviderKey) && (log.LoginProvider == login.LoginProvider))
                .FirstOrDefault();
            if (l != null)
            {
                userDto = Mapper.Map<User, UserDto>(l.User);
            }
                
            return Task.FromResult<UserDto>(userDto);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
                
            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            return Task.FromResult<IList<UserLoginInfo>>(u.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(UserDto user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            var l = u.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);

            this.unitOfWork.GetRepository<User>().Update(u);
            return this.unitOfWork.SaveAsync();
        }
        #endregion

        #region IUserRoleStore<IdentityUser, Guid> Members
        public Task AddToRoleAsync(UserDto user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");
            }
                
            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            Role role = this.unitOfWork.GetRepository<Role>().Get(r => r.Name == roleName).FirstOrDefault();

            if (role == null)
            {
                throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");
            }                

            u.Roles.Add(role);
            this.unitOfWork.GetRepository<User>().Update(u);

            return this.unitOfWork.SaveAsync();
        }

        public Task<IList<string>> GetRolesAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
                
            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(UserDto user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            return Task.FromResult<bool>(u.Roles.Any(x => x.Name == roleName));
        }

        public Task RemoveFromRoleAsync(UserDto user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");
            }                

            User u = this.unitOfWork.GetRepository<User>().Get(us => us.UserId == user.Id).FirstOrDefault();
            if (u == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }                

            var r = u.Roles.FirstOrDefault(x => x.Name == roleName);
            u.Roles.Remove(r);

            this.unitOfWork.GetRepository<User>().Update(u);
            return this.unitOfWork.SaveAsync();
        }
        #endregion

        #region IUserPasswordStore<IdentityUser, Guid> Members
        public Task<string> GetPasswordHashAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
                
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }                
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(UserDto user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserSecurityStampStore<IdentityUser, Guid> Members
        public Task<string> GetSecurityStampAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }                
            return Task.FromResult<string>(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(UserDto user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        #endregion
    }
}
