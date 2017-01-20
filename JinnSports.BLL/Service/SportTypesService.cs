using AutoMapper;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Dtos.SportType;
using JinnSports.BLL.Interfaces;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Service
{
    public class SportTypeService : ISportTypeService
    {
        private const string SPORTCONTEXT = "SportsContext";

        private static readonly ILog Log = LogManager.GetLogger(typeof(EventsService));

        private readonly IUnitOfWork dataUnit;

        public SportTypeService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }
        public int Count(int sportTypeId)
        {
            int count;
            if (sportTypeId != 0)
            {
                count = this.dataUnit.GetRepository<SportEvent>()
                .Get(filter: m => m.SportType.Id == sportTypeId)
                .Count();
            }
            else
            {
                count = this.dataUnit.GetRepository<SportEvent>()
                .Get().Count();
            }
            return count;
        }

        public SportTypeSelectDto GetSportTypes(int sportTypeId, int time)
        {
            IList<ResultDto> results = new List<ResultDto>();
            IList<SportTypeListDto> sportTypeListDtos = new List<SportTypeListDto>();
            ICollection<SportEvent> sportEvents = new List<SportEvent>();
            string selectedName;

            if (sportTypeId != 0)
            {
                SportType sportType = this.dataUnit.GetRepository<SportType>().Get(filter: x => x.Id == sportTypeId).FirstOrDefault();

                selectedName = sportType.Name;

                sportEvents = sportType.SportEvents;

                if (time != 0)
                {
                    sportEvents = sportEvents.Where(m => Math.Sign(DateTime.Compare(m.Date, DateTime.Now))==time).Select(m => m).ToList();
                }

                if (sportEvents.Count() > 0)
                {
                    foreach (SportEvent sportEvent in sportEvents)
                    {
                        results.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
                    }
                    sportTypeListDtos.Add(new SportTypeListDto
                    {
                        SportType = new SportTypeDto
                        {
                            Id = sportType.Id,
                            Name = sportType.Name
                        },
                        Results = results
                    });
                }
            }
            else
            {
                IEnumerable<SportType> sportTypes = this.dataUnit.GetRepository<SportType>().Get();

                selectedName = "Sport Events";

                foreach (SportType sportType in sportTypes)
                {
                    sportEvents = sportType.SportEvents;

                    if (time != 0)
                    {
                        sportEvents = sportEvents.Where(m => Math.Sign(DateTime.Compare(m.Date, DateTime.Now)) == time).Select(m => m).ToList();
                    }

                    if (sportEvents.Count() > 0)
                    {
                        foreach (SportEvent sportEvent in sportEvents)
                        {
                            results.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
                        }

                        sportTypeListDtos.Add(new SportTypeListDto
                        {
                            SportType = new SportTypeDto
                            {
                                Id = sportType.Id,
                                Name = sportType.Name
                            },
                            Results = results
                        });

                        results = new List<ResultDto>();
                    }
                }
            }
            SportTypeSelectDto sportTypeModel = new SportTypeSelectDto()
            {
                SelectedId = sportTypeId,
                SelectedName = selectedName, 
                SportTypes = this.GetAllSportTypes(),
                SportTypeResults = sportTypeListDtos
            };
            return sportTypeModel;
        }

        public IEnumerable<SportTypeDto> GetAllSportTypes()
        {
            IEnumerable<SportType> sportTypes = this.dataUnit.GetRepository<SportType>().Get();
            IList<SportTypeDto> sportTypesDto = new List<SportTypeDto>();

            foreach (SportType sportType in sportTypes)
            {
                sportTypesDto.Add(Mapper.Map<SportType, SportTypeDto>(sportType));
            }
            return sportTypesDto;
        }
    }
}
