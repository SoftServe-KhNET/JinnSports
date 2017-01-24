using JinnSports.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinnSports.WEB.Areas.Mvc.Models
{
    public class TeamDetailsViewModel
    {
        public TeamResultsDto TeamResultDto { get; set; }

        public PageInfo PageInfo { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }
}