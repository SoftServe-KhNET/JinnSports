using JinnSports.BLL;
using JinnSports.BLL.Dtos.SportType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Mvc.Models
{
    public class SportEventViewModel
    {
        public SportTypeSelectDto SportTypeSelectDto { get; set; }

        public PageInfo PageInfo { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public static IDictionary<TimeSelector, string> TimeSelection { get; private set; }

        static SportEventViewModel()
        {
            TimeSelection = new Dictionary<TimeSelector, string>();
            TimeSelection.Add(TimeSelector.All, "Все");
            TimeSelection.Add(TimeSelector.Past, "Прошлое");
            TimeSelection.Add(TimeSelector.Future, "Будущее");
        }
    }
}