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
            this.ConfigureAutoMapper();
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

        private void ConfigureAutoMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<TeamEventDTO, TeamEvent>()
                    .ForMember(
                        e => e.AttackScore,
                        opt => opt.MapFrom(
                            s => s.AttackScore))
                   .ForMember(
                        e => e.DefenseScore,
                        opt => opt.MapFrom(
                            s => s.DefenseScore))
                    .ForMember(
                        e => e.IsHomeGame,
                        opt => opt.MapFrom(
                            s => s.IsHomeGame))
                   .ForMember(
                        e => e.Date,
                        opt => opt.MapFrom(
                            s => new DateTime(s.Date)));
            });
        }
    }
}