using log4net;
using PredictorApplication.Models.Settings;
using ScorePredictor;
using ScorePredictor.EventData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PredictorApplication.Models
{
    public class TaskManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TaskManager));

        private object lockMe = new object();

        public delegate void AddPrediction(Prediction prediction);
        public delegate void Notify();

        public event AddPrediction AddNewPrediction;
        public event Notify NotifySender;

        public IEnumerable<IncomingEvent> IncomingEvents { private get; set; }
        public IList<Prediction> Predictions { get; private set; }

        public void RunPrediction()
        {
            Task predictionTask = new Task(RunParallelPrediction);
            predictionTask.Start();
        }

        private void RunParallelPrediction()
        {
            if (IncomingEvents == null)
            {
                return;
            }

            Parallel.ForEach(IncomingEvents, PredictEvent);
            NotifySender();
        }

        private void PredictEvent(IncomingEvent incomingEvent)
        {
            try
            {
                TeamData homeTeam = null;
                TeamData awayTeam = null;

                Prediction prediction = CreatePrediction(incomingEvent);

                int maxScore = new SettingsReader().ReadMaxScore(incomingEvent.SportType);

                foreach (TeamInfo teamInfo in incomingEvent.TeamsInfo)
                {
                    if (teamInfo.IsHomeGame)
                    {
                        homeTeam = new TeamData(teamInfo.TeamEvents, true);
                    }
                    else
                    {
                        awayTeam = new TeamData(teamInfo.TeamEvents, false);
                    }
                }

                Predictor predictor = new Predictor(homeTeam, awayTeam, maxScore);
                predictor.CalcProbabilities();

                prediction.HomeWinProbability = predictor.HomeWinProbability;
                prediction.DrawProbability = predictor.DrawProbability;
                prediction.AwayWinProbability = predictor.AwayWinProbability;

                AddNewPrediction(prediction);
            }
            catch (Exception ex)
            {
                Log.Error("Exception while trying to PredictEvent", ex);
            }
            
        }

        private Prediction CreatePrediction(IncomingEvent incomingEvent)
        {
            Prediction prediction = new Prediction();
            prediction.IncomingEventId = incomingEvent.Id;

            foreach (TeamInfo teamInfo in incomingEvent.TeamsInfo)
            {
                if (teamInfo.IsHomeGame)
                {
                    prediction.HomeTeamId = teamInfo.TeamId;
                }
                else
                {
                    prediction.AwayTeamId = teamInfo.TeamId;
                }
            }

            return prediction;
        }
    }
}