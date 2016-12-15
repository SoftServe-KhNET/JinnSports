using System.Collections.Generic;
using System.Linq;
using JinnSports.BLL.Exceptions;
using JinnSports.BLL.Interfaces;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.DAL.Repositories;
using JinnSports.Entities.Entities;
using JinnSports.BLL.Dtos;
using System;

namespace JinnSports.BLL.Service
{
    public class TeamService : ITeamService
    {
        private const string SPORTCONTEXT = "SportsContext";

        private IUnitOfWork dataUnit;       

        public int Count()
        {
            int count;
            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                count = this.dataUnit.GetRepository<Team>().Count();
            }
            return count;
        }

        public IEnumerable<TeamDto> GetAllTeams()
        {
            IList<TeamDto> teamDtoList = new List<TeamDto>();

            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                IEnumerable<Team> teams = this.dataUnit.GetRepository<Team>().Get();
                foreach (Team team in teams)
                {
                    TeamDto teamDto = new TeamDto { Name = team.Name };

                    foreach (Result result in team.Results)
                    {
                        teamDto.Results.Add(result.SportEvent.Date, result.Score);
                    }

                    teamDtoList.Add(teamDto);
                }
            }

           return teamDtoList;
        }

        public TeamDto GetTeamById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
