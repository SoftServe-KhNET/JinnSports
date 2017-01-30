using System.Collections.Generic;
using System.Linq;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Dtos;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Collections;
using AutoMapper;
using JinnSports.BLL.Filters;
using System.ComponentModel;

namespace JinnSports.BLL.Service
{
    public class TeamDetailsService : ITeamDetailsService
    {
        private readonly IUnitOfWork dataUnit;

        public TeamDetailsService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }

        private void FilterTeamEvents(IQueryable<SportEvent> teamEvents, TeamDetailsFilter filter)
        {
            teamEvents.Where(e => e.Results.Any(r => r.Team.Id == filter.TeamId));

            if (filter.DateFrom.Ticks > 0)
            {
                teamEvents.Where(e => e.Date >= filter.DateFrom);
            }

            if (filter.DateTo.Ticks > 0)
            {
                teamEvents.Where(e => e.Date <= filter.DateTo);
            }

            if (!string.IsNullOrEmpty(filter.OpponentTeamName))
            {
                teamEvents.Where(e => e.Results.Any(
                    r => r.Team.Name == filter.OpponentTeamName ||
                    r.Team.Names.Any(n => n.Name == filter.OpponentTeamName)));
            }
        }

        private void OrderTeamEvents(IQueryable<SportEvent> teamEvents, TeamDetailsFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.SortedField))
            {
                switch (filter.SortedField)
                {
                    case "FirstTeam":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            teamEvents.OrderBy(
                                e => e.Results.ElementAt(0).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(0).Id)
                                .ThenBy(e => e.Date);
                        }
                        else
                        {
                            teamEvents.OrderByDescending(
                                e => e.Results.ElementAt(0).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(0).Id)
                                .ThenBy(e => e.Date);
                        }
                        break;

                    case "SecondTeam":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            teamEvents.OrderBy(
                                e => e.Results.ElementAt(1).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(1).Id)
                                .ThenBy(e => e.Date);
                        }
                        else
                        {
                            teamEvents.OrderByDescending(
                                e => e.Results.ElementAt(1).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(1).Id)
                                .ThenBy(e => e.Date);
                        }
                        break;

                    case "Date":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            teamEvents.OrderBy(e => e.Date).ThenBy(e => e.Id);
                        }
                        else
                        {
                            teamEvents.OrderByDescending(e => e.Date).ThenBy(e => e.Id);
                        }
                        break;

                    default:
                        teamEvents.OrderBy(e => e.Date).ThenBy(e => e.Id);
                        break;
                }
            }
            else
            {
                teamEvents.OrderBy(e => e.Date).ThenBy(e => e.Id);
            }
        }

        public int Count(TeamDetailsFilter filter)
        {
            IQueryable<SportEvent> teamEvents =
                this.dataUnit.GetRepository<SportEvent>().Get();

            this.FilterTeamEvents(teamEvents, filter);            

            return teamEvents.Count();
        }

        public IEnumerable<ResultDto> GetResults(TeamDetailsFilter filter)
        {
            List<ResultDto> results = new List<ResultDto>();            

            IQueryable<SportEvent> teamEvents = this.dataUnit.GetRepository<SportEvent>().Get(
                includeProperties: "Team,SportEvent,SportEvent.Results,SportEvent.Results.Team");

            this.FilterTeamEvents(teamEvents, filter);
            this.OrderTeamEvents(teamEvents, filter);
            

            if (filter.Page > 0 && filter.PageSize > 0)
            {
                teamEvents.Skip((filter.Page - 1) * filter.PageSize);
                teamEvents.Take(filter.PageSize);
            }


            foreach (SportEvent teamEvent in teamEvents)
            {
                results.Add(Mapper.Map<SportEvent, ResultDto>(teamEvent));
            }

            return results;
        }
    }
}
