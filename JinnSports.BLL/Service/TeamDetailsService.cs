using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Dtos;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Collections;

namespace JinnSports.BLL.Service
{
    public class TeamDetailsService : ITeamDetailsService
    {
        private const string SPORTCONTEXT = "SportsContext";

        private IUnitOfWork dataUnit;

        public int Count(int? teamId)
        {
            int count;

            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                count = this.dataUnit.GetRepository<Team>().GetById(teamId).Results.ToList().Count(); //изменить
            }
            return count;
        }
        
        public IEnumerable<ResultDto> GetResults(int teamId)
        {
            List<ResultDto> orderedTeamResults = new List<ResultDto>();
            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                Team team = this.dataUnit.GetRepository<Team>().GetById(teamId);
                Result res = this.dataUnit.GetRepository<Result>().GetById(9);

                IEnumerable teamResults = team.Results.ToList();

                foreach (Result teamResult in teamResults)
                {
                    IEnumerable<Result> eventResults = teamResult.SportEvent.Results;
                    ResultDto resultDto = new ResultDto();
                    /*foreach (Result result in eventResults) здесь сделаем переход к листу элементов
                    {
                        resultDto.Teams.Add(result.Team.Name);
                    }*/
                    resultDto.Id = teamResult.Id;
                    resultDto.Score = string.Format("{0} : {1}", eventResults.ElementAt(0).Score, eventResults.ElementAt(1).Score);
                    resultDto.TeamFirst = eventResults.ElementAt(0).Team.Name;
                    resultDto.TeamSecond = eventResults.ElementAt(1).Team.Name;
                    resultDto.TeamFirstId = eventResults.ElementAt(0).Team.Id;
                    resultDto.TeamSecondId = eventResults.ElementAt(1).Team.Id;
                    resultDto.Date = teamResult.SportEvent.Date;
                    orderedTeamResults.Add(resultDto);
                }
            }
            return orderedTeamResults;
        }
    }
}
