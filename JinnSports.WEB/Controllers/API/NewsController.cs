using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JinnSports.WEB.Controllers.Api
{
    public class NewsController : ApiController
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        [HttpGet]
        public IHttpActionResult LoadNews()
        {
            IEnumerable<NewsDto> newsDtos = this.newsService.GetLastNews();
            return this.Ok(new
            {
                data = newsDtos
            });
        }
    }
}
