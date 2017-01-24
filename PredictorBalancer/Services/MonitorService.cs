using AutoMapper;
using PredictorBalancer.Models;
using PredictorBalancer.ViewModels;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictorBalancer.Services
{
    public class MonitorService
    {
        private BalancerMonitor monitor;

        public IEnumerable<PredictorViewModel> GetPredicors()
        {
            this.monitor = BalancerMonitor.GetInstance();
            IEnumerable<Predictor> predictors = this.monitor.Predictors.GetAll();

            IEnumerable<PredictorViewModel> viewPredictors = Mapper.Map<IEnumerable<Predictor>, IEnumerable<PredictorViewModel>>(predictors);
            return viewPredictors;
        }
    }
}