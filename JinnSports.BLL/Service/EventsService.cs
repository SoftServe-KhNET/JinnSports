using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;

namespace JinnSports.BLL.Service
{
    public class EventsService : IEventService
    {
        private IUnitOfWork dataUnit;

        public IList<CompetitionEventDto> GetCEvents()
        {
            IList<CompetitionEventDto> events = new List<CompetitionEventDto>();
            string competitionEventResult = string.Empty;

            this.dataUnit = new EFUnitOfWork("SportsContext");

            IRepository<CompetitionEvent> eventsRepository = this.dataUnit.Set<CompetitionEvent>();
            IRepository<Team> teams = this.dataUnit.Set<Team>();
            IRepository<Result> results = this.dataUnit.Set<Result>();

            IEnumerable<CompetitionEvent> competitionEvents = eventsRepository.GetAll();
            foreach (CompetitionEvent ce in competitionEvents)
            {
                CompetitionEventDto competitionEvent = new CompetitionEventDto();
                competitionEvent.Date = ce.Date;

                var datedResults = results.GetAll(r => r.CompetitionEvent.Id == ce.Id);

                competitionEventResult = string.Empty;

                foreach (Result res in datedResults)
                {
                    Team selectedTeam = teams.Get(t => t.Id == res.Team.Id);
                    if (string.IsNullOrEmpty(competitionEvent.SportType)) 
                    {
                        competitionEvent.SportType = selectedTeam.SportType.Name;
                    }

                    if (string.IsNullOrEmpty(competitionEvent.Team1))
                    {
                        competitionEvent.Team1 = res.Team.Name;
                    }
                    else
                    {
                        competitionEvent.Team2 = res.Team.Name;
                    }

                    if (string.IsNullOrEmpty(competitionEvent.Result1))
                    {
                        competitionEvent.Result1 = res.Score;
                    }
                    else
                    {
                        competitionEvent.Result2 = res.Score;
                    }
                }
                events.Add(competitionEvent);
            }
            return events;
        }

        public void Dispose()
        {
            this.dataUnit.Dispose();
        }
    }
}
