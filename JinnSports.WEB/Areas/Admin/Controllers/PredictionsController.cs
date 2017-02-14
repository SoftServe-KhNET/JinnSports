using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JinnSports.BLL.Interfaces;
using PredictorDTO;
using JinnSports.BLL.Service;
using System.Web.Configuration;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PredictionsController : ApiController
    {
        private readonly PredictionsService predictionsService;

        public PredictionsController(PredictionsService predictionsService)
        {
            this.predictionsService = predictionsService;
        }

        [AllowAnonymous]
        public IHttpActionResult PostPredictions(IEnumerable<PredictionDTO> predictions)
        {
            IEnumerable<string> values = new List<string>();
            bool serviceCode = Request.Headers.TryGetValues(WebConfigurationManager.AppSettings["ApiKey"], out values);

            if (!serviceCode)
            {
                return this.BadRequest();
            }
            else
            {
                this.predictionsService.SavePredictions(predictions);
                return this.Ok();
            }
            
        }
    }
}
