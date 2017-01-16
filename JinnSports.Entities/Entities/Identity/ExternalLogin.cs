using System;

namespace JinnSports.Entities.Entities.Identity
{
    public class ExternalLogin
    {
        private User user;

        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual Guid UserId { get; set; }

        public virtual User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                UserId = value.UserId;
            }
        }        
    }
}