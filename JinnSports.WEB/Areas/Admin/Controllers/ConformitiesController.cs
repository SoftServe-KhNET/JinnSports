using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ConformitiesController : Controller
    {
        private readonly IAdminService conformityService;

        public ConformitiesController(IAdminService conformityService)
        {
            this.conformityService = conformityService;
        }
        
        public ActionResult Index()
        {
            var model = this.conformityService.GetConformities();            
               
            return this.View(model);
        }
        
        public ActionResult Edit(int id)
        {
            var model = this.conformityService.GetConformityViewModel(id);
            return this.View(model);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ConformityViewModel model)
        {
            var errors = ModelState.Values;
            if (!ModelState.IsValid)
            {
                var newModel = this.conformityService
                    .GetConformityViewModel(model.InputNameId);

                return this.View("Edit", newModel);
            }

            this.conformityService.Save(model);

            return this.RedirectToAction("Index");
        }
    }
}
