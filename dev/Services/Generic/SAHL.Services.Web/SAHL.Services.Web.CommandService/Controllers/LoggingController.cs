using SAHL.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public class LoggingController : Controller
    {
        ILoggerSourceManager loggerSourceManager;

        public LoggingController(ILoggerSourceManager loggerSourceManager)
        {
            this.loggerSourceManager = loggerSourceManager;
        }

        // GET: Logging
        public ActionResult Index()
        {
            IDictionary<Guid, ILoggerSource> sources = this.loggerSourceManager.AvailableSources;
            var list = sources.Values.GroupBy(x => x.Name);
            List<SelectListItem> selectItems = new List<SelectListItem>();
            foreach (var s in list)
            {
                SelectListItem b = new SelectListItem();
                b.Value = s.Key;
                b.Text = s.Key;
                selectItems.Add(b);
            }
            ViewBag.LoggingSources = new SelectList(selectItems);
            return View(sources);
        }

        [HttpPost]
        public JsonResult ChangeLogLevel(Guid key, string logLevel)
        {
            this.loggerSourceManager.AvailableSources[key].LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevel);
            return Json(string.Format("success: {0}-{1}", key, logLevel), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeLogMetrics(Guid key, bool logMetrics)
        {
            this.loggerSourceManager.AvailableSources[key].LogMetrics = logMetrics;
            return Json(string.Format("success: {0}-{1}", key, logMetrics), JsonRequestBehavior.AllowGet);
        }
    }
}