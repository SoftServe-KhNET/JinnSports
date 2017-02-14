using PredictorBalancer.Models;
using PredictorBalancer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PredictorBalancer
{
    public class HomeController : Controller
    {
        private MonitorService monitorService;

        public async Task<ActionResult> Index()
        {
            this.monitorService = new MonitorService();
            return this.View(await this.monitorService.GetPredicors());
        }

        public ActionResult Add(string baseUrl, string controllerUrn, int timeoutSec)
        {
            this.monitorService = new MonitorService();
            this.monitorService.Add(baseUrl, controllerUrn, timeoutSec);
            return this.RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            this.monitorService = new MonitorService();
            this.monitorService.Delete(id);
            return this.RedirectToAction("Index");
        }

    }
}
