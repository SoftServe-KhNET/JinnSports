using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Service
{
    public class PredictoionSender
    {
        private readonly IUnitOfWork dataUnit;

        private IList<IncomingEventDTO> incomingEvents;

        private Task sendRequest;

        public PredictoionSender(IUnitOfWork dataUnit)
        {
            this.dataUnit = dataUnit;
        }

        public void SendPredictionRequest()
        {
            sendRequest = new Task(CreateRequest);
            sendRequest.Start();
        }

        private void CreateRequest()
        {
            CheckForNewEvents();
            if (incomingEvents.Count > 0)
            {
                PackageDTO package = new PackageDTO();
                package.IncomigEvents = incomingEvents;

                // TODO: get connection parameters from settings
                package.CallBackURL = "";
                package.CallBackController = "api/Predictions";
                package.CallBackTimeout = 60;

                ApiConnection connection = new ApiConnection();
                connection.SendPackage(package);
            }
        }

        private void CheckForNewEvents()
        {
            using (dataUnit)
            {
                DateTime currentDate = DateTime.Today;
                IEnumerable<SportEvent> events = dataUnit.GetRepository<SportEvent>().Get(e => e.Date >= currentDate);
                IEnumerable<EventPrediction> eventPredictions = dataUnit.GetRepository<EventPrediction>().Get();

                foreach (SportEvent sportEvent in events)
                {
                    if (sportEvent.Results.FirstOrDefault(r => r.Score == -1) != null)
                    {
                        // Check if prediction for given event already exists
                        if (eventPredictions.FirstOrDefault(ep => ep.SportEvent.Id == sportEvent.Id) != null)
                        {
                            continue;
                        }
                        AddIncomingEvent(sportEvent);
                    }
                }
            }   
        }

        private void AddIncomingEvent(SportEvent sportEvent)
        {
            if (incomingEvents == null)
            {
                incomingEvents = new List<IncomingEventDTO>();
            }

            IncomingEventDTO incomingEvent = new IncomingEventDTO();
            incomingEvent.Id = sportEvent.Id;
            incomingEvent.SportType = sportEvent.SportType.Name;

            foreach (Result result in sportEvent.Results)
            {
                TeamInfoDTO teamInfo = new TeamInfoDTO();
                teamInfo.IsHomeGame = result.IsHome;
                teamInfo.TeamId = result.Team.Id;
                teamInfo.TeamEvents = GetTeamEvents(result.Team.Results, result.Team.Id);
            }

            incomingEvents.Add(incomingEvent);
        }

        private IEnumerable<TeamEventDTO> GetTeamEvents(IEnumerable<Result> results, int teamId)
        {
            IList<TeamEventDTO> teamEvents = new List<TeamEventDTO>();

            foreach (Result result in results)
            {
                TeamEventDTO teamEvent = new TeamEventDTO();
                teamEvent.Date = result.SportEvent.Date.Ticks;

                foreach (Result item in result.SportEvent.Results)
                {
                    if (item.Team.Id == teamId)
                    {
                        teamEvent.IsHomeGame = item.IsHome;
                        teamEvent.AttackScore = item.Score;
                    }
                    else
                    {
                        teamEvent.DefenceScore = item.Score;
                    }  
                }

                teamEvents.Add(teamEvent);
            }

            return teamEvents;
        }
    }
}
