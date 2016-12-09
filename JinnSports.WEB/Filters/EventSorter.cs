using System.Collections.Generic;
using System.Linq;
using JinnSports.BLL.Dtos;
using JinnSports.WEB.Mappers;
using JinnSports.WEB.Models;

namespace JinnSports.WEB.Filters
{
    internal static class EventSorter
    {
        public static IList<SportResultsViewModel> Sort(IList<CompetitionEventDto> compEvents)
        {
            List<SportResultsViewModel> viewModels = new List<SportResultsViewModel>();

            var groupBySportQuery = from c in compEvents
                                    group c by c.SportType;

            foreach (var events in groupBySportQuery)
            {
                var orderByDateQuery = from c in events
                                       orderby c.Date descending
                                       select c;

                SportResultsViewModel currentModel = new SportResultsViewModel { SportName = events.Key };
                currentModel.Events = new List<ResultViewModel>();
                foreach (var result in orderByDateQuery)
                {
                    currentModel.Events.Add(result.MapToViewModel());
                }
                viewModels.Add(currentModel);
            }

            return viewModels;
        }
    }
}