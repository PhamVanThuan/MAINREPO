using SAHL.Core.BusinessModel;
using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Commands;
using SAHL.Services.Interfaces.Search;
using SAHL.Services.Interfaces.Search.Commands;
using SAHL.Services.Interfaces.Search.Models;
using System;
using System.Web.Mvc;

namespace SAHL.Website.Halo.Controllers
{
    public class ClientController : HaloController
    {
        private IHaloService haloService;
        private ISearchService searchService;
        private const int DefaultPageSize = 10;

        public ClientController(ISearchService searchService, IHaloService haloService)
        {
            this.searchService = searchService;
            this.haloService = haloService;
        }

        public ActionResult Index()
        {
            var changeCommand = new ChangeRibbonMenuSelectionCommand(this.User.Identity.Name, Request.RawUrl);
            haloService.PerformCommand(changeCommand);

            return View();
        }

        public void RemoveBusinessContext(string context, long businessKey, string businessKeyType)
        {
            var removeCommand = new RemoveRibbonMenuItemCommand(this.User.Identity.Name, context, businessKey, businessKeyType);
            var result = haloService.PerformCommand(removeCommand);
        }

        public ActionResult Search()
        {
            //this is the default route so set a url that we can use to find in the menu
            string url = string.Join("/", RouteData.Values.Values);

            var changeCommand = new ChangeRibbonMenuSelectionCommand(this.User.Identity.Name, url);
            haloService.PerformCommand(changeCommand);

            return View();
        }

        [HttpPost]
        public ActionResult Search(ClientQuery clientQuery)
        {
            clientQuery.PageSize = clientQuery.PageSize > 0 ? clientQuery.PageSize : DefaultPageSize;
            var command = new GetClientResultFromMultiFieldSearchCommand(clientQuery);
            var messageCollection = searchService.PerformCommand(command);
            var queryResult = command.Result;

            return PartialView("_SearchResultsPartial", queryResult);
        }

        [HttpPost]
        public MvcHtmlString GetSearchResults(ClientQuery clientQuery)
        {
            var command = new GetClientResultFromMultiFieldSearchCommand(clientQuery);
            var messageCollection = searchService.PerformCommand(command);
            return new MvcHtmlString(Render(this, "_SearchResultsPartial", command.Result));
        }

        public ActionResult SearchResult(Client client)
        {
            string url = Url.Action("Index", "Client", new { context = "Client", businessKey = client.ID, businessKeyType = BusinessKeyType.LegalEntity });
            var command = new AddRibbonMenuItemCommand(this.User.Identity.Name, client.ID, BusinessKeyType.LegalEntity, client.DisplayName, url, "Client");
            var result = haloService.PerformCommand(command);
            var businessContext = command.Result;

            return RedirectToAction("Index", "Client", new { context = businessContext.Context, businessKey = businessContext.BusinessKey.Key, businessKeyType = businessContext.BusinessKey.KeyType });
        }
    }
}