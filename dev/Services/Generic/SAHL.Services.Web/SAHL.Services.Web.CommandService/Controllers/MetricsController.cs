using SAHL.Core.Services.Metrics;
using SAHL.Services.Web.CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public class MetricsController : Controller
    {
        ICommandServiceRequestMetrics commandServiceMetrics;

        public MetricsController(ICommandServiceRequestMetrics commandServiceMetrics)
        {
            this.commandServiceMetrics = commandServiceMetrics;
        }

        public ActionResult Index()
        {
            return View(commandServiceMetrics);
        }

        public ActionResult All()
        {
            return Page(0);
        }

        public ActionResult Page(int page)
        {
            var selectedItems = commandServiceMetrics.CommandLatencyForLastDay.OrderByDescending(x => x.Value.Mean).Select(kvp => kvp.Key).Skip(page * 20).Take(20);
            int count = (int)Math.Ceiling(commandServiceMetrics.CommandLatencyForLastDay.Count / (double)20);
            PageModel model = new PageModel(selectedItems, page, count);
            return View("All",model);
        }

        public ActionResult Top20()
        {
            var selectedItems = commandServiceMetrics.CommandLatencyForLastDay.OrderByDescending(x => x.Value.Mean).Select(kvp => kvp.Key).Take(20);
            int count = (int)Math.Ceiling(commandServiceMetrics.CommandLatencyForLastDay.Count / (double)20);
            PageModel model = new PageModel(selectedItems, 0, count);
            return PartialView("_SelectedCommands", model);
        }

        public ActionResult GetCommand(string name)
        {
            Command c = new Command(this.commandServiceMetrics,name);
            return PartialView("_Command", c);
        }
    }
}
