using SAHL.Core.UI.Context;
using SAHL.Core.UI.Models;
using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SAHL.Website.Halo.Controllers
{
    public class EditorController : HaloController
    {
        private IHaloService haloService;

        public EditorController(IHaloService haloService)
        {
            this.haloService = haloService;
        }

        public MvcHtmlString ShowEditor(TileBusinessContext tileBusinessContext)
        {
            var command = new GetEditorForUserCommand(User.Identity.Name, tileBusinessContext);
            var messageCollection = haloService.PerformCommand(command);
            var editorElement = command.Result;
            var renderedTemplate = Render(this, "_EditorPartial", editorElement);

            return new MvcHtmlString(renderedTemplate);
        }

        public MvcHtmlString GetPreviousPageModel(EditorBusinessContext editorBusinessContext)
        {
            var command = new GetPreviousEditorPageContentForUserCommand(User.Identity.Name, editorBusinessContext);
            var messageCollection = haloService.PerformCommand(command);
            var editorPageContent = command.Result;

            var renderedTemplate = Render(this, "_EditorPageModelPartial", editorPageContent);
            return new MvcHtmlString(renderedTemplate);
        }

        public MvcHtmlString GetPageModel(EditorBusinessContext editorBusinessContext)
        {
            var command = new GetEditorPageContentForUserCommand(User.Identity.Name, editorBusinessContext);
            var messageCollection = haloService.PerformCommand(command);
            var editorPageContent = command.Result;

            var renderedTemplate = Render(this, "_EditorPageModelPartial", editorPageContent);
            return new MvcHtmlString(renderedTemplate);
        }

        public JsonResult SubmitEditorPage(EditorBusinessContext editorBusinessContext, string editorPageModelTypeName, string pageModel)
        {
            Type editorPageModelType = Type.GetType(editorPageModelTypeName);

            pageModel = pageModel.StripAway(@"\w*\.");

            IEditorPageModel pageModelToValidate = (IEditorPageModel)Newtonsoft.Json.JsonConvert.DeserializeObject(pageModel, editorPageModelType);

            var command = new SubmitEditorPageForUserCommand(User.Identity.Name, editorBusinessContext, pageModelToValidate);
            var messageCollection = haloService.PerformCommand(command);
            var validationResults = command.Result;

            Response.StatusCode = validationResults.Count() > 0 ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;

            return new JsonResult
            {
                Data = validationResults,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SubmitEditor(EditorBusinessContext editorBusinessContext, string editorPageModelTypeName, string pageModel)
        {
            Type editorPageModelType = Type.GetType(editorPageModelTypeName);

            pageModel = pageModel.StripAway(@"\w*\.");

            IEditorPageModel pageModelToValidate = (IEditorPageModel)Newtonsoft.Json.JsonConvert.DeserializeObject(pageModel, editorPageModelType);

            var command = new SubmitEditorForUserCommand(User.Identity.Name, editorBusinessContext, pageModelToValidate);
            var messageCollection = haloService.PerformCommand(command);
            var validationResults = command.Result;

            Response.StatusCode = validationResults.Count() > 0 ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;

            return new JsonResult
            {
                Data = validationResults,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public class Validation : IUIValidationResult
    {
        public string Message { get; set; }

        public string PropertyName { get; set; }

        public Core.UI.Enums.ValidationSeverityLevel Severity { get; set; }
    }

    public static class StringExtensions
    {
        public static string StripAway(this string stringToUse, string pattern)
        {
            Regex regex = new Regex(pattern);
            var matches = regex.Matches(stringToUse);
            foreach (var match in matches) { stringToUse = stringToUse.Replace(match.ToString(), String.Empty); }
            return stringToUse;
        }
    }
}