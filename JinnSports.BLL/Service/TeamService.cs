using System.Collections.Generic;
using System.Linq;
using JinnSports.BLL.Interfaces;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using JinnSports.BLL.Dtos;
using AutoMapper;
using JinnSports.BLL.Filters;
using System.ComponentModel;

namespace JinnSports.BLL.Service
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork dataUnit;

        public TeamService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }

        private void FilterTeams(ref IQueryable<Team> teams, TeamFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.TeamName))
            {
                teams = teams.Where(
                    t => t.Name == filter.TeamName ||
                    t.Names.Any(n => n.Name == filter.TeamName));
            }
        }

        private void OrderTeams(ref IQueryable<Team> teams, TeamFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.SortedField))
            {
                switch (filter.SortedField)
                {
                    case "SportType":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            teams = teams.OrderBy(t => t.SportType.Name)
                                .ThenBy(t => t.Name)
                                .ThenBy(t => t.Id);
                        }
                        else
                        {
                            teams = teams.OrderByDescending(t => t.SportType.Name)
                                .ThenBy(t => t.Name)
                                .ThenBy(t => t.Id);
                        }
                        break;

                    case "TeamName":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            teams = teams.OrderBy(t => t.Name).ThenBy(t => t.Id);
                        }
                        else
                        {
                            teams = teams.OrderByDescending(t => t.Name).ThenBy(t => t.Id);
                        }
                        break;

                    default:
                        teams = teams.OrderBy(t => t.Name).ThenBy(t => t.Id);
                        break;
                }
            }
            else
            {
                teams = teams.OrderBy(t => t.Name).ThenBy(t => t.Id);
            }
        }

        public int Count(TeamFilter filter)
        {
            IQueryable<Team> teams = this.dataUnit.GetRepository<Team>().Get();
            this.FilterTeams(ref teams, filter);           

            return teams.Count();
        }

        public IEnumerable<TeamDto> GetTeams(TeamFilter filter)
        {
            IList<TeamDto> teamDtoList = new List<TeamDto>();            
            IQueryable<Team> teams = 
                this.dataUnit.GetRepository<Team>().Get(includeProperties:"SportType");

            this.FilterTeams(ref teams, filter);
            this.OrderTeams(ref teams, filter);

            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.PageSize < 1)
            {
                filter.PageSize = 10;
            }
                        
            teams = teams.Skip((filter.Page - 1) * filter.PageSize);
            teams = teams.Take(filter.PageSize);

            foreach (Team team in teams)
            {
                teamDtoList.Add(Mapper.Map<Team, TeamDto>(team));
            }

            return teamDtoList;
        }

        public TeamDto GetTeamById(int teamId)
        {
            Team team = this.dataUnit.GetRepository<Team>().GetById(teamId);
            TeamDto teamDto = Mapper.Map<Team, TeamDto>(team);
            return teamDto;
        }
    }
}
