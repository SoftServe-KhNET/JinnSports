using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.DAL.Repositories;
using JinnSports.WEB.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;
using JinnSports.WEB.Filters;

namespace JinnSports.WEB.Controllers
{
    public class EventController : Controller
    {
        public ActionResult History()
        {
            IEventService bllService = new EventsService();
            IList<CompetitionEventDto> events = bllService.GetCEvents();
            IList<SportResultsViewModel> viewModel = EventSorter.Sort(events);
            var unit = new EFUnitOfWork("SportsContext");
            var a = unit.Set<Result>().GetAll();
            bllService.Dispose();

            return this.View(viewModel);
        }
    }
}