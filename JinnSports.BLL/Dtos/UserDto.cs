using Microsoft.AspNet.Identity;
using System;

namespace JinnSports.BLL.Dtos
{
    public class UserDto : IUser<Guid>
    {
        public UserDto()
        {
            this.Id = Guid.NewGuid();
        }

        public UserDto(string userName)
            : this()
        {
            this.UserName = userName;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
    }
}
