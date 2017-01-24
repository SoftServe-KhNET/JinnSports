using PredictorBalancer.Models;
using PredictorBalancer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PredictorBalancer
{
    public class HomeController : Controller
    {
        private MonitorService monitorService;

        public ActionResult Index()
        {
            this.monitorService = new MonitorService();
            return this.View(this.monitorService.GetPredicors());
        }

    }
}
