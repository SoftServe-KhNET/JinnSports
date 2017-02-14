using JinnSports.BLL.Dtos;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using PredictorDTO;
using System;
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

        public IEnumerable<EventPredictionDto> GetPredictions()
        {
            IList<EventPredictionDto> predictions = new List<EventPredictionDto>();

            foreach (EventPrediction prediction in this.dataUnit.GetRepository<EventPrediction>().Get())
            {
                EventPredictionDto predictionDto = new EventPredictionDto();
                predictionDto.EventType = prediction.SportEvent.SportType.Name;
                predictionDto.EventDate = prediction.SportEvent.Date.ToShortDateString();
                predictionDto.HomeTeamName = prediction.HomeTeam.Name;
                predictionDto.AwayTeamName = prediction.AwayTeam.Name;
                predictionDto.HomeWinProbability = Convert.ToString(Math.Round(prediction.HomeWinProbability * 10));
                predictionDto.AwayWinProbability = Convert.ToString(Math.Round(prediction.AwayWinProbability * 10));
                predictionDto.DrawProbability = Convert.ToString(Math.Round(prediction.DrawProbability * 10));

                predictions.Add(predictionDto);
            }

            return predictions;
        }

        public void SavePredictions(IEnumerable<PredictionDTO> predictions)
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