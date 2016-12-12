﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JinnSports.Entities.Entities
{
    public class SportEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public virtual SportType SportType { get; set; }
        public virtual ICollection<Result> Results { get; set; }

        public override bool Equals(object obj)
        {
            if (Date == null || SportType == null || Results == null)
            {
                return false;
            }

            if (obj == null)
            {
                return false;
            }

            SportEvent sportEvent = obj as SportEvent;
            if ((object)sportEvent == null)
            {
                return false;
            }

            return (Date == sportEvent.Date) && (SportType.Name == sportEvent.SportType.Name) && CheckResults(sportEvent.Results);
        }

        public bool Equals(SportEvent sportEvent)
        {
            if (Date == null || SportType == null || Results == null)
            {
                return false;
            }

            if ((object)sportEvent == null)
            {
                return false;
            }

            return (Date == sportEvent.Date) && (SportType.Name == sportEvent.SportType.Name) && CheckResults(sportEvent.Results);
        }

        public override int GetHashCode()
        {
            int hashCode = Date.GetHashCode() ^ SportType.Name.GetHashCode();
            List<string> teamNames = Results.Select(r => r.Team.Name).ToList();

            foreach (string teamName in teamNames)
            {
                hashCode ^= teamName.GetHashCode();
            }

            return hashCode;
        }

        public override string ToString()
        {
            return $"Id: {Id}; Date: {Date}";
        }

        private bool CheckResults(ICollection<Result> foreignResults)
        {
            List<string> thisTeamNames = Results.Select(r => r.Team.Name).ToList();
            List<string> foreignTeamNames = foreignResults.Select(r => r.Team.Name).ToList();

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
    }
}