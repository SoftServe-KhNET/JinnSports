using AutoMapper;
using PredictorBalancer.Models;
using PredictorBalancer.ViewModels;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PredictorBalancer.Services
{
    public class MonitorService
    {
        private BalancerMonitor monitor;

        public async Task<IEnumerable<PredictorViewModel>> GetPredicors()
        {
            this.monitor = BalancerMonitor.GetInstance();
            // TODO: Resolve async issue
            await this.monitor.UpdateStatus();
            IEnumerable<Predictor> predictors = this.monitor.Predictors.GetAll();

            IEnumerable<PredictorViewModel> viewPredictors = Mapper.Map<IEnumerable<Predictor>, IEnumerable<PredictorViewModel>>(predictors);
            return viewPredictors;
        }

        public void Delete(int id)
        {
            this.monitor = BalancerMonitor.GetInstance();
            this.monitor.Predictors.Delete(id);
        }

        public void Add(string baseUrl, string controllerUrn, int timeoutSec)
        {
            this.monitor = BalancerMonitor.GetInstance();
            this.monitor.Predictors.Add(baseUrl, controllerUrn, timeoutSec);
        }
    }
}