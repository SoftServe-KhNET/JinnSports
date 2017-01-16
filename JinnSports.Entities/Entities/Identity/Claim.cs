using System;

namespace JinnSports.Entities.Entities.Identity
{
    public class Claim
    {
        private User user;               

        public virtual int ClaimId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
        
        public virtual User User
        {
            get
            {
                return user;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }                    
                user = value;
                UserId = value.UserId;
            }
        }        
    }
}
