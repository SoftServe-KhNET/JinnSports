using PredictorBalancer.Models;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PredictorBalancer.Controllers
{
    public class PredictionsController : ApiController
    {
        private BalancerMonitor instance;

        public void PostPredictions([FromBody]IEnumerable<PredictionDTO> predictions)
        {
            this.instance = BalancerMonitor.GetInstance();
            this.instance.SendPredictions(predictions);
        }
    }
}
