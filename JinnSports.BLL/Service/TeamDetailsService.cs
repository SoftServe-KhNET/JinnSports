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
using JinnSports.BLL.Extentions;
using AutoMapper;
using JinnSports.BLL.Mapping;

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

                IEnumerable teamResults = team.Results.ToList();

                var mappingConfig = new AutoMapper.MapperConfiguration(cfg =>
                {
                    cfg.AddServiceMapping();
                });

                IMapper mapper = mappingConfig.CreateMapper();
                foreach (Result teamResult in teamResults)
                {
                    IEnumerable<Result> eventResults = teamResult.SportEvent.Results;
                    ResultDto resultDto = new ResultDto();

                    orderedTeamResults.Add(mapper.Map<Result, ResultDto>(teamResult));
                }
            }
            return orderedTeamResults;
        }
    }
}
