using JinnSports.BLL.Dtos;
using JinnSports.BLL.Service;
using JinnSports.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Controllers
{

    public class EventPredictionController : Controller
    {
        private readonly PredictionsService predictionsService;

        public EventPredictionController(PredictionsService predictionsService)
        {
            this.predictionsService = predictionsService;
        }

        public ActionResult Index()
        {
            IEnumerable<EventPredictionDto> predictions = this.predictionsService.GetPredictions();
            if (predictions != null)
            {
                return this.View(predictions);
            }
            else
            {
                return this.HttpNotFound();
            }
        }

    }
}