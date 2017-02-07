using JinnSports.BLL.Dtos;
using JinnSports.BLL.Dtos.SportType;
using JinnSports.BLL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinnSports.WEB.Areas.Mvc.Models
{
    public class SportEventViewModel
    {
        public IEnumerable<ResultDto> Results { get; set; }

        public IEnumerable<SportTypeDto> SportTypes { get; set; }

        public PageInfo PageInfo { get; set; }

        public SportEventFilter Filter { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }
}