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
            IsAwailable = true;
            ConfigureAutoMapper();
        }

        public bool IsAwailable { get; private set; }
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
                IsAwailable = false;
                IEnumerable<IncomingEvent> incomingEvents = Mapper.Map<IEnumerable<IncomingEventDTO>, IEnumerable<IncomingEvent>>(incomingEventsDTO);
                taskManager = new TaskManager();
                resultManger = new ResultManager();

                resultManger.NotifyChangeStatus += ChangeStatus;
                taskManager.AddNewPrediction += resultManger.AddPrediction;
                taskManager.NotifySender += resultManger.SendPredictions;

                taskManager.IncomingEvents = incomingEvents;
                taskManager.RunPrediction();
            }
            catch (Exception ex)
            {
                // TODO: notify about failure
                IsAwailable = true;
                Log.Error("Exception while trying to RunPrediction", ex);
            }
        }

        private void ChangeStatus()
        {
            IsAwailable = true;
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
                        e => e.DefenceScore,
                        opt => opt.MapFrom(
                            s => s.DefenceScore))
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