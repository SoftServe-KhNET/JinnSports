using System;
using System.Collections.Generic;

namespace JinnSports.Entities.Entities.Identity
{
    public class User
    {               
        private ICollection<ExternalLogin> externalLogins;
        private ICollection<Role> roles;
        
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }  

        public virtual ICollection<ExternalLogin> Logins
        {
            get
            {
                return this.externalLogins ??
                    (this.externalLogins = new List<ExternalLogin>());
            }
            set
            {
                this.externalLogins = value;
            }
        }

        public virtual ICollection<Role> Roles
        {
            get { return this.roles ?? (this.roles = new List<Role>()); }
            set { this.roles = value; }
        }        
    }
}
