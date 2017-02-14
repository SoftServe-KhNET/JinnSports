using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.WEB.Filters;
using System.Collections.Generic;
using System.Web.Http;


namespace JinnSports.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ConformityController : ApiController
    {
        private readonly IAdminService adminService;

        public ConformityController(IAdminService conformityService)
        {
            this.adminService = conformityService;
        }
                
        [HttpGet]
        public IHttpActionResult GetConformities()
        {
            AdminApiViewModel model = this.adminService.GetConformityApiViewModel();
            return this.Ok(model);                                        
        }

        [ValidateCustomAntiForgeryToken]
        [HttpPost]
        public IHttpActionResult PostConformities([FromBody]List<ConformityDto> model)
        {
            foreach (ConformityDto conf in model)
            {
                this.adminService.Save(conf.InputName, conf.ExistedName);
            }
            return this.Ok();
        }
    }
}
