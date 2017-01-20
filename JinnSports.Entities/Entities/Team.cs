using JinnSports.Entities.Entities.Temp;
using System.Collections.Generic;
using System.Linq;

namespace JinnSports.Entities.Entities
{
    public class Team
    {
        public int Id { get; set; }
        
        public virtual SportType SportType { get; set; }

        public string Name { get; set; }

        public ICollection<TeamName> Names { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public virtual ICollection<TempResult> TempResults { get; set; }

        public bool Equals(Team team)
        {
            if (this.Names == null || this.SportType == null)
            {
                return false;
            }

            if ((object)team == null || team.Names == null || team.SportType == null)
            {
                return false;
            }

            return (this.Names == team.Names) && (SportType.Name == team.SportType.Name) && this.CheckNames(team.Names);
        }

        public override int GetHashCode()
        {
            if (this.Names == null || this.SportType == null)
            {
                return 0;
            }

            int hashCode = this.Names.GetHashCode() ^ SportType.Name.GetHashCode();
            List<string> teamNames = this.Names.Select(r => r.Name).ToList();

            foreach (string teamName in teamNames)
            {
                hashCode ^= teamName.GetHashCode();
            }

            return hashCode;
        }  

        public override string ToString()
        {
            return $"Id: {Id}; Name: {Name}; SportType: {SportType.Name}";
        }

        private bool CheckNames(ICollection<TeamName> foreignNames)
        {
            if (this.HasNullNames(this.Names) || this.HasNullNames(foreignNames))
            {
                return false;
            }

            List<string> thisTeamNames = this.Names.Select(r => r.Name).ToList();
            List<string> foreignTeamNames = foreignNames.Select(r => r.Name).ToList();

            if (thisTeamNames.Count != foreignTeamNames.Count)
            {
                return false;
            }

            foreach (string teamName in thisTeamNames)
            {
                if (!foreignTeamNames.Contains<string>(teamName))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasNullNames(ICollection<TeamName> names)
        {
            foreach (TeamName name in names)
            {
                if (name == null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
