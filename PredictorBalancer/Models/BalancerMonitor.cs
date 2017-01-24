using AutoMapper;
using log4net;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PredictorBalancer.Models
{
    public class BalancerMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BalancerMonitor));

        private static BalancerMonitor instance;

        private BalancerMonitor()
        {

        }

        public Notifier Notifier { get; private set; }
        public Package Package { get; private set; }
        public PredictorList Predictors { get; private set; }

        public static BalancerMonitor GetInstance()
        {
            if (instance == null)
            {
                instance = new BalancerMonitor();
                instance.Notifier = new Notifier();
                instance.Predictors = new PredictorList();
            }

            return instance;
        }

        public void SendPredictions(IEnumerable<PredictionDTO> predictions)
        {
            ApiConnection<IEnumerable<PredictionDTO>> connection = new ApiConnection<IEnumerable<PredictionDTO>>(Package.CallBackURL,
                                                                        Package.CallBackController, Package.CallBackTimeout);
            connection.Send(predictions);
        }

        public void SendIncomingEvents(PackageDTO package, string baseUrl)
        {
            try
            {
                Package = Mapper.Map<PackageDTO, Package>(package);

                this.UpdateStatus();
                IEnumerable<Predictor> predictors = this.GetavailablePredictors();

                if (predictors.Count() == 0)
                {
                    Notifier.SendEmail("All predictors are unavailable. Task aborted.");
                    return;
                }

                // Divide incomingEvents to equal parts for all available predictors 
                int size = (package.IncomigEvents.Count() / predictors.Count()) + (package.IncomigEvents.Count() % predictors.Count());
                int count = 0;

                foreach (Predictor predictor in predictors)
                {
                    predictor.SendIncomingEvents(this.CreatePackage(package.IncomigEvents.Skip(count * size).Take(size), baseUrl));
                    count++;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception occured while trying to send incomingEvents", ex);
            }
            
        }

        private PackageDTO CreatePackage(IEnumerable<IncomingEventDTO> incomingEvents, string baseUrl)
        {
            return new PackageDTO
            {
                IncomigEvents = incomingEvents,
                CallBackURL = baseUrl,
                CallBackController = $"api/Balancer",
                CallBackTimeout = 60
            };
        }

        private void UpdateStatus()
        {
            int count = 0;
            Task[] tasks = new Task[this.Predictors.GetAll().Count]; 

            foreach (Predictor predictor in this.Predictors.GetAll())
            {
                tasks[count] = Task.Factory.StartNew(() => predictor.UpdateStatus());
                count++;
            }

            Task.WaitAll(tasks);
        }

        private IEnumerable<Predictor> GetavailablePredictors()
        {
            return this.Predictors.GetAll().Where(p => p.CurrentStatus == Predictor.Status.Available).ToList();
        }
    }
}