using PredictorApplication.Models;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PredictorApplication.Controllers
{
    public class PredictionController : ApiController
    {
        private PredictorMonitor monitor;

        public IHttpActionResult GetStatus()
        {
            this.monitor = PredictorMonitor.GetInstance();

            if (!this.monitor.IsAvailable)
            {
                return this.ResponseMessage(this.Request.CreateResponse(HttpStatusCode.ServiceUnavailable));
            }

            return this.Ok();
        }

        public IHttpActionResult Post([FromBody] PackageDTO package)
        {
            this.monitor = PredictorMonitor.GetInstance();

            if (!this.monitor.IsAvailable)
            {
                return this.ResponseMessage(this.Request.CreateResponse(HttpStatusCode.ServiceUnavailable));
            }

            this.monitor.CallBackURL = package.CallBackURL;
            this.monitor.CallBackController = package.CallBackController;
            this.monitor.CallBackTimeout = package.CallBackTimeout;
            this.monitor.RunPredictionTask(package.IncomigEvents);

            return this.Ok();
        }
    }
}
