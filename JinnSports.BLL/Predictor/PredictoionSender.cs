using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;

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
            this.sendRequest = new Task(this.CreateRequest);
            this.sendRequest.Start();
        }

        private void CreateRequest()
        {
            this.CheckForNewEvents();
            if (this.incomingEvents.Count > 0)
            {
                PackageDTO package = new PackageDTO();
                package.IncomigEvents = this.incomingEvents;

                // TODO: get connection parameters from settings
                this.GetConnectionSettings(package);

                ApiConnection connection = new ApiConnection();
                connection.SendPackage(package);
            }
        }

        private void GetConnectionSettings(PackageDTO package)
        {
            XmlDocument settings = new XmlDocument();
            settings.Load(HostingEnvironment.MapPath("~/App_Data/") + "PredictionsConnection.xml");
            package.CallBackURL = settings.DocumentElement.SelectSingleNode("url").InnerText;
            package.CallBackController = settings.DocumentElement.SelectSingleNode("name").InnerText;
            package.CallBackTimeout = int.Parse(settings.DocumentElement.SelectSingleNode("timeout").InnerText ?? "60");
        }

        private void CheckForNewEvents()
        {
            using (this.dataUnit)
            {
                DateTime currentDate = DateTime.Today;
                IEnumerable<SportEvent> events = this.dataUnit.GetRepository<SportEvent>().Get(e => e.Date >= currentDate);
                IEnumerable<EventPrediction> eventPredictions = this.dataUnit.GetRepository<EventPrediction>().Get();

                foreach (SportEvent sportEvent in events)
                {
                    if (sportEvent.Results.FirstOrDefault(r => r.Score == -1) != null)
                    {
                        // Check if prediction for given event already exists
                        if (eventPredictions.FirstOrDefault(ep => ep.SportEvent.Id == sportEvent.Id) != null)
                        {
                            continue;
                        }
                        this.AddIncomingEvent(sportEvent);
                    }
                }
            }   
        }

        private void AddIncomingEvent(SportEvent sportEvent)
        {
            if (this.incomingEvents == null)
            {
                this.incomingEvents = new List<IncomingEventDTO>();
            }

            IncomingEventDTO incomingEvent = new IncomingEventDTO();
            incomingEvent.Id = sportEvent.Id;
            incomingEvent.SportType = sportEvent.SportType.Name;

            foreach (Result result in sportEvent.Results)
            {
                TeamInfoDTO teamInfo = new TeamInfoDTO();
                teamInfo.IsHomeGame = result.IsHome;
                teamInfo.TeamId = result.Team.Id;
                teamInfo.TeamEvents = this.GetTeamEvents(result.Team.Results, result.Team.Id);
            }

            this.incomingEvents.Add(incomingEvent);
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
                        teamEvent.DefenseScore = item.Score;
                    }  
                }

                teamEvents.Add(teamEvent);
            }

            return teamEvents;
        }
    }
}
