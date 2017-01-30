using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using DTO.JSON;
using JinnSports.BLL.Dtos.SportType;
using JinnSports.BLL.Filters;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Counts events for sport type
        /// </summary>        
        /// <returns></returns>                        
        int Count(SportEventFilter filter);
        
        /// <summary>
        /// Get events for sport type
        /// </summary>
        /// <param name="sportId">Sport type ID</param>
        /// <param name="time"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<ResultDto> GetSportEvents(SportEventFilter filter);

        MainPageDto GetMainPageInfo();

        bool SaveSportEvents(ICollection<SportEventDTO> eventDTOs);

        IEnumerable<SportTypeDto> GetSportTypes();
    }
}