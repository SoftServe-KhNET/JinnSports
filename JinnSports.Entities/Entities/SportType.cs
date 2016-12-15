using System.Collections.Generic;

namespace JinnSports.Entities.Entities
{
    public class SportType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<SportEvent> SportEvents { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; Name: {Name}";
        }
    }
}
