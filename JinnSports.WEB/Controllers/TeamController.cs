using System.Collections.Generic;
using System.Web.Mvc;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using System.Linq;
using JinnSports.BLL.Dtos;
using System;

namespace JinnSports.WEB.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team        
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
