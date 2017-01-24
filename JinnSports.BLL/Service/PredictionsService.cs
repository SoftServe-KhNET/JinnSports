using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using PredictorDTO;
using System.Collections.Generic;
using System.Linq;

namespace JinnSports.BLL.Service
{
    public class PredictionsService
    {
        private readonly IUnitOfWork dataUnit;

        public PredictionsService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }

        public void SavePredictions(IEnumerable<PredictionDTO> predictions)
        {
            using (this.dataUnit)
            {
                IEnumerable<Team> teams = this.dataUnit.GetRepository<Team>().Get();
                IEnumerable<SportEvent> sportEvents = this.dataUnit.GetRepository<SportEvent>().Get();

                // TODO: Exception handling
                foreach (PredictionDTO predictionDTO in predictions)
                {
                    EventPrediction prediction = new EventPrediction();

                    prediction.SportEvent = sportEvents.FirstOrDefault(se => se.Id == predictionDTO.IncomingEventId);
                    prediction.HomeTeam = teams.FirstOrDefault(t => t.Id == predictionDTO.HomeTeamId);
                    prediction.AwayTeam = teams.FirstOrDefault(t => t.Id == predictionDTO.AwayTeamId);

                    prediction.HomeWinProbability = predictionDTO.HomeWinProbability;
                    prediction.AwayWinProbability = predictionDTO.AwayWinProbability;
                    prediction.DrawProbability = predictionDTO.DrawProbability;

                    this.dataUnit.GetRepository<EventPrediction>().Insert(prediction);
                }

                this.dataUnit.SaveChanges();
            }
        }

    }
}