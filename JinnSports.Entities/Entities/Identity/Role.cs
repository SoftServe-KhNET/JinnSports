using System;
using System.Collections.Generic;

namespace JinnSports.Entities.Entities.Identity
{
    public class Role
    {        
        private ICollection<User> users; 
        
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        
        public ICollection<User> Users
        {
            get { return users ?? (users = new List<User>()); }
            set { users = value; }
        }        
    }
}
