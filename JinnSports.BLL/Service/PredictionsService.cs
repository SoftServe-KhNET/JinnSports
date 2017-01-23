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
    public class PredictionsService
    {
        private readonly IUnitOfWork dataUnit;

        public PredictionsService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }

        public void SavePredictions(IEnumerable<PredictionDTO> predictions)
        {
            using (dataUnit)
            {
                IEnumerable<Team> teams = dataUnit.GetRepository<Team>().Get();
                IEnumerable<SportEvent> sportEvents = dataUnit.GetRepository<SportEvent>().Get();

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

                    dataUnit.GetRepository<EventPrediction>().Insert(prediction);
                }

                dataUnit.SaveChanges();
            }
        }

    }
}
