using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using SAHL.Core.Web.Services;
using SAHL.Core.Services.CommandPersistence;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public class CommandReplayController : Controller
    {
        private readonly ICommandSessionFactory factory;
        private readonly IJsonActivator jsonActivator;
        private readonly IHttpCommandReRun commandReRuner;

        public CommandReplayController(ICommandSessionFactory factory, IJsonActivator jsonActivator, IHttpCommandReRun commandReRuner)
        {
            this.factory        = factory;
            this.jsonActivator  = jsonActivator;
            this.commandReRuner = commandReRuner;
        }

        // GET: CommandReplay
        public ActionResult Index()
        {
            IEnumerable<ICommandSession> items = factory.GetAllFailedAndPending(DeserialiseTheContextDetails);
            List<ICommandSession> itemsList = items.Take(50).ToList();
            return View(itemsList);
        }

        public JsonResult ReplayCommand(string Key)
        {
            var commandKey     = int.Parse(Key);
            var commandSession = factory.CreateNewCommandManager(commandKey, DeserialiseTheContextDetails);
            commandReRuner.TryRunCommand(commandSession);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        private IContextDetails DeserialiseTheContextDetails(string serialisedContextDetails)
        {
            var jsonString = serialisedContextDetails.Replace("_name", "$type");
            return jsonActivator.DeserializeObject<IContextDetails>(jsonString);
        }
        
    }
}
