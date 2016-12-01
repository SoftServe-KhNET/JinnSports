using System;
using System.Collections.Generic;

namespace JinnSports.DAL.Entities
{
    public class SportType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
