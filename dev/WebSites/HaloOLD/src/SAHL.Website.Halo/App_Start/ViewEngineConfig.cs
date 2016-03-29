using System;
using System.Linq;
using System.Web.Mvc;

namespace SAHL.Website.Halo
{
	public static class ViewEngineConfig
	{
		public static void RegisterViewEngine(ViewEngineCollection viewEngines)
		{
			viewEngines.Clear();
			viewEngines.Add(new HaloViewEngine());
		}
	}

	public class HaloViewEngine : RazorViewEngine
	{
		private string[] templateModes = new string[] { "DisplayTemplates/", "EditorTemplates/" };
		private string[] namespaces;

		public HaloViewEngine()
		{
			namespaces = new[]{
				@"SAHL.Core.UI."
			};

			var partialViewLocationFormats = this.PartialViewLocationFormats.ToList();
			partialViewLocationFormats.Add("~/Views/Shared/Partials/{0}.cshtml");
			this.PartialViewLocationFormats = partialViewLocationFormats.ToArray();
		}

		public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
		{
			if (templateModes.Any(x => partialViewName.Contains(x)))
			{
				return FindDisplayTemplate(controllerContext, partialViewName, useCache);
			}
			return base.FindPartialView(controllerContext, partialViewName, useCache);
		}

		private ViewEngineResult FindDisplayTemplate(ControllerContext controllerContext, string modelTypeName, bool useCache)
		{
			foreach (var assemblyName in namespaces)
			{
				var partialViewName = modelTypeName.Replace(assemblyName, String.Empty).Replace('.', '/');
				var viewResult = base.FindPartialView(controllerContext, partialViewName, useCache);
				if (viewResult.View != null)
					return viewResult;
			}
			return base.FindPartialView(controllerContext, modelTypeName, useCache);
		}
	}
}