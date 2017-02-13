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
    public class BalancerController : ApiController
    {
        private BalancerMonitor instance;

        public IHttpActionResult PostPackage([FromBody]PackageDTO package)
        {
            this.instance = BalancerMonitor.GetInstance();
            this.instance.SendIncomingEvents(package, Request.RequestUri.GetLeftPart(UriPartial.Authority));
            return this.Ok();
        }
    }
}
