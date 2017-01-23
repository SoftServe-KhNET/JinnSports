using AutoMapper;
using PredictorDTO;
using ScorePredictor.EventData;
using System.Collections.Generic;

namespace PredictorApplication.Models
{
    public class ResultManager
    {
        private IList<PredictionDTO> predictions;
        private object lockMe = new object();

        public delegate void NotifyMonitor();
        public event NotifyMonitor NotifyChangeStatus;

        public void AddPrediction(Prediction prediction)
        {
            lock (lockMe)
            {
                if (predictions == null)
                {
                    predictions = new List<PredictionDTO>();
                }

                PredictionDTO predictionDTO = Mapper.Map<Prediction, PredictionDTO>(prediction);
                predictions.Add(predictionDTO);
            }
        }

        public void SendPredictions()
        {
            ApiConnection sender = new ApiConnection();
            sender.SendPredictions(predictions);
            NotifyChangeStatus();
        }
    }
}