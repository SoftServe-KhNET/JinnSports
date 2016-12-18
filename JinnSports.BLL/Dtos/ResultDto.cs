﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Dtos
{
    public class ResultDto
    {
        public int Id { get; set; }
        public string Score { get; set; }
        public DateTime Date { get; set; }
        public int TeamFirstId { get; set; }
        public string TeamFirst { get; set; }
        public int TeamSecondId { get; set; }
        public string TeamSecond { get; set; }

        public bool Equals(ResultDto resultDto)
        {
            if (this.Date == null || string.IsNullOrEmpty(Score) || Id < 1 || TeamFirstId < 1 || TeamSecondId < 1 || 
                string.IsNullOrEmpty(TeamFirst) || string.IsNullOrEmpty(TeamSecond) || resultDto == null)
            {
                return false;
            }

            return (this.Date == resultDto.Date) && (this.Score == resultDto.Score) && 
                (this.Id == resultDto.Id) && (this.TeamFirstId == resultDto.TeamFirstId) && 
                (this.TeamSecondId == resultDto.TeamSecondId) && (this.TeamFirst == resultDto.TeamFirst) &&
                (this.TeamSecond == resultDto.TeamSecond);
        }

        /*public override int GetHashCode()
        {
            if (Date == null || SportType == null || SportType.Name == null)
            {
                return 0;
            }

            int hashCode = Date.GetHashCode() ^ SportType.Name.GetHashCode();
            List<string> teamNames = Results.Select(r => r.Team.Name).ToList();

            foreach (string teamName in teamNames)
            {
                hashCode ^= teamName.GetHashCode();
            }

            return hashCode;
        }*/
    }
}