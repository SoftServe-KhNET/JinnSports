using System;
using System.Collections.Generic;

namespace JinnSports.Entities.Entities
{
    public class CompetitionEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return "Id: " + this.Id + " Date: " + this.Date;
        }
    }
}
