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
            var count = this.dataUnit.GetRepository<Team>().GetById(teamId).Results.ToList().Count;

            return count;
        }

        public IEnumerable<ResultDto> GetResults(int teamId, int skip, int take)
        {
            List<ResultDto> orderedTeamResults = new List<ResultDto>();

            Team team = this.dataUnit.GetRepository<Team>().GetById(teamId);

            IEnumerable teamResults = team.Results.OrderByDescending(x => x.SportEvent.Date).ThenByDescending(x => x.SportEvent.Id).Skip(skip).Take(take).ToList();

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
