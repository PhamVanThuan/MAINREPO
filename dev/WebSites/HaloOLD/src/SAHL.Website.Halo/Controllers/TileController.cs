using Newtonsoft.Json;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Commands;
using SAHL.Website.Halo.Attributes;
using System;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SAHL.Website.Halo.Controllers
{
    public class TileController : HaloController
    {
        private IHaloService haloService;

        public TileController(IHaloService haloService)
        {
            this.haloService = haloService;
        }

        public MvcHtmlString GetClientMenu(string context, int businessKey, string businessKeyType)
        {
            var command = new GetUserMenuCommand(this.User.Identity.Name);
            var result = haloService.PerformCommand(command);
            var menuArea = command.Result;
            var renderedTemplate = Render(this, "_ContextMenuPartial", menuArea.ContextBar);
            return new MvcHtmlString(renderedTemplate);
        }

        public MvcHtmlString GetUsersTilesForContext(TileBusinessContext tileBusinessContext)
        {
            var command = new GetUsersTilesForContextCommand(tileBusinessContext, this.User.Identity.Name);
            var result = haloService.PerformCommand(command);
            var userTileElementArea = command.Result;
            var renderedTemplate = Render(this, "_TileElementAreaPartial", userTileElementArea);
            return new MvcHtmlString(renderedTemplate);
        }

        public MvcHtmlString DrillDownAndGetUsersTilesForContext(TileBusinessContext tileBusinessContext)
        {
            var command = new DrillDownAndGetUsersTilesForContextCommand(tileBusinessContext, this.User.Identity.Name);
            var result = haloService.PerformCommand(command);
            var userTileElementArea = command.Result;
            var renderedTemplate = Render(this, "_TileElementAreaPartial", userTileElementArea);
            return new MvcHtmlString(renderedTemplate);
        }

        [ActionSessionState(SessionStateBehavior.ReadOnly)]
        public MvcHtmlString GetTileData(TileBusinessContext tileBusinessContext)
        {
            var command = new GetTileContentForContextCommand(tileBusinessContext);
            var result = haloService.PerformCommand(command);
            var tileData = command.Result;
            var renderedTemplate = Render(this, "_TileModelPartial", tileData);
            return new MvcHtmlString(renderedTemplate);
        }

        [ActionSessionState(SessionStateBehavior.ReadOnly)]
        public JsonResult GetDrillPreviewData(TileBusinessContext tileBusinessContext)
        {
            var command = new GetTileContentForContextCommand(tileBusinessContext);
            var result = haloService.PerformCommand(command);
            var tileData = command.Result;
            var json = JsonConvert.SerializeObject(tileData, new JsonConverter[] { new Newtonsoft.Json.Converters.StringEnumConverter() });
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}