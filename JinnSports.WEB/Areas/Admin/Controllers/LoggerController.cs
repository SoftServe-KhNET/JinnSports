using JinnSports.BLL.Dtos.ClientLog;
using JinnSports.BLL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    public class LoggerController : ApiController
    {
        private readonly ILoggerService loggerService;

        public LoggerController(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        [HttpPost]
        public IHttpActionResult Post(HttpRequestMessage value)
        {
            var jsonString = value.Content.ReadAsStringAsync();
            string json = jsonString.Result;
            LogDto log = JsonConvert.DeserializeObject<LogDto>(json);
            this.loggerService.SaveLog(log);
            return Ok();
        }
    }
}