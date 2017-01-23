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
            monitor = PredictorMonitor.GetInstance();

            if (!monitor.IsAwailable)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.ServiceUnavailable));
            }

            return Ok();
        }

        public IHttpActionResult Post([FromBody] PackageDTO package)
        {
            monitor = PredictorMonitor.GetInstance();

            if (!monitor.IsAwailable)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.ServiceUnavailable));
            }

            monitor.CallBackURL = package.CallBackURL;
            monitor.CallBackController = package.CallBackController;
            monitor.CallBackTimeout = package.CallBackTimeout;
            monitor.RunPredictionTask(package.IncomigEvents);

            return Ok();
        }
    }
}
