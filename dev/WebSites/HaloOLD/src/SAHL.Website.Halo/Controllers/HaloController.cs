using SAHL.Website.Halo.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace SAHL.Website.Halo.Controllers
{
    public class HaloController : Controller
    {
        protected void ShowNotification(Notification notification)
        {
            if (Session["Notifications"] == null)
            {
                Session["Notifications"] = new List<Notification>();
            }
            var notifications = Session["Notifications"] as List<Notification>;
            notifications.Add(notification);
        }

        public string Render(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var stringWriter = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);

                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }
}