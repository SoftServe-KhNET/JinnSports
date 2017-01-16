using System;
using System.Collections.Generic;

namespace JinnSports.Entities.Entities.Identity
{
    public class User
    {       
        private ICollection<Claim> claims;
        private ICollection<ExternalLogin> externalLogins;
        private ICollection<Role> roles;
        
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        
        public virtual ICollection<Claim> Claims
        {
            get { return claims ?? (claims = new List<Claim>()); }
            set { claims = value; }
        }

        public virtual ICollection<ExternalLogin> Logins
        {
            get
            {
                return externalLogins ??
                    (externalLogins = new List<ExternalLogin>());
            }
            set
            {
                externalLogins = value;
            }
        }

        public virtual ICollection<Role> Roles
        {
            get { return roles ?? (roles = new List<Role>()); }
            set { roles = value; }
        }        
    }
}
