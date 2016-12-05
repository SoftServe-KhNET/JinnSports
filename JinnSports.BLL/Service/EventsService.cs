using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JinnSports.BLL.Interfaces;
using JinnSports.DataAccessInterfaces;
using JinnSports.DAL.Repositories;
using JinnSports.Entities;
using JinnSports.BLL.Entities;

namespace JinnSports.BLL.Service
{
    public class EventsService : IEventService
    {
        public IList<CEvent> GetCEvents()
        {
            IList<CEvent> events = new List<CEvent>();
            string competitionEventResult = string.Empty;

            IUnitOfWork dataUnit = new EFUnitOfWork();

            IRepository<CompetitionEvent> eventsRepository = dataUnit.Set<CompetitionEvent>();
            IRepository<Team> teams = dataUnit.Set<Team>();
            IRepository<Result> results = dataUnit.Set<Result>();

            IList<CompetitionEvent> competitionEvents = eventsRepository.GetAll();
            foreach (CompetitionEvent ce in competitionEvents)
            {
                CEvent cEvent = new CEvent();
                cEvent.Date = ce.Date;

                var datedResults = results.GetAll(r => r.CompetitionEvent.Id == ce.Id);

                competitionEventResult = string.Empty;
                foreach (Result res in datedResults)
                {
                    Team selectedTeam = teams.Get(t => t.Id == res.Team.Id);
                    if (!cEvent.SportType.Any())
                    {
                        cEvent.SportType = selectedTeam.SportType.ToString();
                    }

                    if (!competitionEventResult.Any())
                    {
                        competitionEventResult = selectedTeam.Name + " " + res.Score;
                    } 
                    else
                    {
                        competitionEventResult += " : " + res.Score + " " + selectedTeam.Name;
                    }
                }
                cEvent.Result = competitionEventResult;
                events.Add(cEvent);
            }
            return events;
        }
    }
}
