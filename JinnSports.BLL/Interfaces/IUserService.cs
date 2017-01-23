using JinnSports.BLL.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JinnSports.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<UserDto> FindAsync(string userName, string password);
        Task<UserDto> FindAsync(UserLoginInfo login);
        Task<IdentityResult> CreateAsync(UserDto user, string password);
        Task<IdentityResult> CreateAsync(UserDto user);
        Task<IdentityResult> RemoveLoginAsync(Guid userId, UserLoginInfo login);
        Task<IdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<IdentityResult> AddPasswordAsync(Guid userId, string password);
        Task<IdentityResult> AddLoginAsync(Guid userId, UserLoginInfo login);
        IList<UserLoginInfo> GetLogins(Guid userId);
        Task<ClaimsIdentity> CreateIdentityAsync(UserDto user, string authenticationType);
        UserDto FindById(Guid userId);
        bool HasPassword(IPrincipal user);
        Guid GetGuid(string value);
    }
}
