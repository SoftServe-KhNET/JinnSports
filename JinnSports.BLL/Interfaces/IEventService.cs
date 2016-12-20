using System.Collections.Generic;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Метод подсчёта количества результатов в заданном виде спорта
        /// </summary>
        /// <param name="sport"></param>
        /// <returns></returns>                        
        int Count(string sport);
        
        /// <summary>
        /// Метод выдачи результатов по заданному виду спорта
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<SportEventDto> GetSportEvents(string sport, int skip, int take);
    }
}
