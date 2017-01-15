using System.Collections.Generic;
using System.Linq;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Dtos;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Collections;
using AutoMapper;

namespace JinnSports.BLL.Service
{
    public class TeamDetailsService : ITeamDetailsService
    {
        private readonly IUnitOfWork dataUnit;

        public TeamDetailsService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }

        public int Count(int teamId)
        {
            int count = this.dataUnit.GetRepository<Result>().Count(r => r.Team.Id == teamId);

            return count;
        }

        public IEnumerable<ResultDto> GetResults(int teamId, int skip, int take)
        {
            List<ResultDto> orderedTeamResults = new List<ResultDto>();            

            IEnumerable<Result> teamResults = this.dataUnit.GetRepository<Result>().Get(
                filter: res => res.Team.Id == teamId,
                includeProperties: "Team,SportEvent,SportEvent.Results,SportEvent.Results.Team",
                orderBy: s => s.OrderByDescending(x => x.SportEvent.Date)
                    .ThenByDescending(t => t.SportEvent.Id),
                skip: skip,
                take: take);

            foreach (Result teamResult in teamResults)
            {
                IEnumerable<Result> eventResults = teamResult.SportEvent.Results;
                ResultDto resultDto = new ResultDto();

                orderedTeamResults.Add(Mapper.Map<Result, ResultDto>(teamResult));
            }

            return orderedTeamResults;
        }
    }
}
