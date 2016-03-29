using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Web.Mvc;

namespace SAHL.Website.Halo.Controllers
{
    public class MyTasksController : HaloController
    {
        private IHaloService haloService;

        public MyTasksController(IHaloService haloService)
        {
            this.haloService = haloService;
        }

        public ActionResult Index()
        {
            var changeCommand = new ChangeRibbonMenuSelectionCommand(this.User.Identity.Name, Request.RawUrl);
            haloService.PerformCommand(changeCommand);

            return View();
        }
    }
}