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

namespace JinnSports.BLL.Service
{
    public class SportTypeService : ISportTypeService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventsService));

        private readonly IUnitOfWork dataUnit;

        public SportTypeService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }

        public int Count(int sportTypeId, int time)
        {
            int count;
            if (sportTypeId != 0)
            {
                if (time != 0)
                {
                    count = this.dataUnit.GetRepository<SportEvent>()
                    .Get(filter: m => m.SportType.Id == sportTypeId && 
                    DateTime.Compare(m.Date, DateTime.UtcNow) == time)
                    .Count();
                }
                else
                {
                    count = this.dataUnit.GetRepository<SportEvent>()
                    .Get(filter: m => m.SportType.Id == sportTypeId)
                    .Count();
                }
            }
            else
            {
                if (time != 0)
                {
                    count = this.dataUnit.GetRepository<SportEvent>()
                    .Get(filter: m => DateTime.Compare(m.Date, DateTime.UtcNow) == time)
                    .Count();
                }
                else
                {
                    count = this.dataUnit.GetRepository<SportEvent>()
                    .Get().Count();
                }
            }
            return count;
        }

        public SportTypeSelectDto GetSportTypes(int sportTypeId, int time, int skip, int take)
        {
            IList<ResultDto> results = new List<ResultDto>();
            IList<SportTypeListDto> sportTypeListDtos = new List<SportTypeListDto>();
            ICollection<SportEvent> sportEvents = new List<SportEvent>();
            string selectedName;

            if (sportTypeId != 0)
            {
                if (time != 0)
                {
                    sportEvents =
                        this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => m.SportType.Id == sportTypeId &&
                        DateTime.Compare(m.Date, DateTime.UtcNow) == time,
                        includeProperties: "Results,SportType,Results.Team",
                        orderBy: s => s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip, 
                        take: take).ToList();
                }
                else
                {
                    sportEvents = this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: x => x.SportType.Id == sportTypeId,
                        includeProperties: "Results,SportType,Results.Team",
                        orderBy: s => s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip, 
                        take: take).ToList();
                }


                if (sportEvents.Count > 0)
                {
                    SportType sportType = sportEvents.ElementAt(0).SportType;
                    selectedName = sportType.Name;
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
                else
                {
                    SportType selectedSportType = this.dataUnit.GetRepository<SportType>().Get(
                        filter: s => s.Id == sportTypeId).FirstOrDefault();
                    if (selectedSportType == null)
                    {
                        selectedName = string.Empty;
                    }
                    else
                    {
                        selectedName = selectedSportType.Name;
                    }
                }
            }
            else
            {
                if (time != 0)
                {
                    sportEvents =
                        this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => DateTime.Compare(m.Date, DateTime.UtcNow) == time,
                        includeProperties: "Results,SportType,Results.Team",
                        orderBy: s => s.OrderBy(x => x.SportType.Id).ThenByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip,
                        take: take).ToList();
                }
                else
                {
                    sportEvents = this.dataUnit.GetRepository<SportEvent>().Get(                        
                        includeProperties: "Results,SportType,Results.Team",
                        orderBy: s => s.OrderBy(x => x.SportType.Id).ThenByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip,
                        take: take).ToList();
                }

                selectedName = "Sport Events";

                if (sportEvents.Count != 0)
                {
                    SportType sportType = sportEvents.ElementAt(0).SportType;

                    foreach (SportEvent sportEvent in sportEvents)
                    {
                        if (sportEvent.SportType.Id != sportType.Id)
                        {
                            sportTypeListDtos.Add(new SportTypeListDto
                            {
                                SportType = new SportTypeDto
                                {
                                    Id = sportType.Id,
                                    Name = sportType.Name
                                },
                                Results = results
                            });
                            sportType = sportEvent.SportType;
                            results = new List<ResultDto>();
                        }
                        results.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
                    }
                    if (results.Count > 0)
                    {
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
            }

            SportTypeSelectDto sportTypeModel = new SportTypeSelectDto()
            {
                SelectedId = sportTypeId,
                SelectedName = selectedName,
                SelectedTime = time,
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