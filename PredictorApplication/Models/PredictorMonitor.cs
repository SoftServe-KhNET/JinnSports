using AutoMapper;
using log4net;
using PredictorDTO;
using ScorePredictor.EventData;
using System;
using System.Collections.Generic;

namespace PredictorApplication.Models
{
    public class PredictorMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PredictorMonitor));

        private static PredictorMonitor instance;

        private TaskManager taskManager;
        private ResultManager resultManger;

        private PredictorMonitor()
        {
            this.IsAvailable = true;
        }

        public bool IsAvailable { get; private set; }
        public string CallBackURL { get; set; }
        public string CallBackController { get; set; }
        public int CallBackTimeout { get; set; }

        public static PredictorMonitor GetInstance()
        {
            if (instance == null)
            {
                instance = new PredictorMonitor();
            }

            return instance;
        }

        public void RunPredictionTask(IEnumerable<IncomingEventDTO> incomingEventsDTO)
        {
            try
            {
                this.IsAvailable = false;
                IEnumerable<IncomingEvent> incomingEvents = Mapper.Map<IEnumerable<IncomingEventDTO>, IEnumerable<IncomingEvent>>(incomingEventsDTO);
                this.taskManager = new TaskManager();
                this.resultManger = new ResultManager();

                this.resultManger.NotifyChangeStatus += this.ChangeStatus;
                this.taskManager.AddNewPrediction += this.resultManger.AddPrediction;
                this.taskManager.NotifySender += this.resultManger.SendPredictions;

                this.taskManager.IncomingEvents = incomingEvents;
                this.taskManager.RunPrediction();
            }
            catch (Exception ex)
            {
                // TODO: notify about failure
                this.IsAvailable = true;
                Log.Error("Exception while trying to RunPrediction", ex);
            }
        }

        private void ChangeStatus()
        {
            this.IsAvailable = true;
        }
    }
}