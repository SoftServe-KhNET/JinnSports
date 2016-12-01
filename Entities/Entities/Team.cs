using System;
using System.Collections.Generic;

namespace JinnSports.DAL.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public virtual SportType SportType { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Result> Results { get; set; }
    }
}
