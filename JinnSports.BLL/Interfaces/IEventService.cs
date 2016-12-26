using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using DTO.JSON;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Counts events for sport type
        /// </summary>
        /// <param name="sportId">Sport type ID</param>
        /// <returns></returns>                        
        int Count(int sportId);
        
        /// <summary>
        /// Get events for sport type
        /// </summary>
        /// <param name="sportId">Sport type ID</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<ResultDto> GetSportEvents(int sportId, int skip, int take);

        bool SaveSportEvents(ICollection<SportEventDTO> eventDTOs);
    }
}
