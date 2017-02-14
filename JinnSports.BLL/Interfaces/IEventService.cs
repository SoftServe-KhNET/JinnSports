using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using DTO.JSON;
using JinnSports.BLL.Dtos.SportType;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Counts events for sport type
        /// </summary>
        /// <param name="sportId">Sport type ID</param>
        /// <returns></returns>                        
        int Count(int sportId, int time);
        
        /// <summary>
        /// Get events for sport type
        /// </summary>
        /// <param name="sportId">Sport type ID</param>
        /// <param name="time"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<ResultDto> GetSportEvents(int sportId, int time, int skip, int take);

        MainPageDto GetMainPageInfo();

        bool SaveSportEvents(ICollection<SportEventDTO> eventDTOs);

        IEnumerable<SportTypeDto> GetSportTypes();

        void RunPredictions();
    }
}