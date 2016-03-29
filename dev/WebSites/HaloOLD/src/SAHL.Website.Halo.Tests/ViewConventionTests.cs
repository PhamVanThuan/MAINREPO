using NUnit.Framework;
using SAHL.Core.UI.Halo.Editors.LegalEntity.CreateHelpDeskCaseEditor.Pages;
using SAHL.Core.UI.Halo.Tiles.LegalEntity.Default;
using SAHL.Core.UI.Models;
using SAHL.Website.Halo.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SAHL.Website.Halo.Tests
{
    [TestFixture]
    public class ViewConventionTests
    {
        [Test, TestCaseSource(typeof(ViewConventionTests), "GetTileModels")]
        public void EnsureViewForEachTileModelExists(Type type)
        {
            //Get the Path we need to check
            //Example: DispalyTemplates/Halo/Tiles/LegalEntity/Default/LegalEntityDetailsMajorTileModel
            string path = type.Namespace.Replace("SAHL.Core.UI.", "");
            path = path.Replace(".", "/");
            path = "DisplayTemplates/" + path;
            path += "/" + type.Name;

            TestingViewEngine engine = new TestingViewEngine();
            var viewResult = engine.FindPartialView(null, path, false);

            Assert.IsNotNull(viewResult.View, String.Format("TileModel {0} does not have a view. Expected a view in ~/Views/Shared/{1}", type.Name, path));
        }

        public class TestingViewEngine : RazorViewEngine
        {
            public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
            {
                //Get the project's directory path.
                var webProjectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Path.Combine("../../../",
                   typeof(CommonController).Assembly.GetName().Name)));

                //The views should only ever be in the Views/Shared/ folder, so we only check in this location.
                var fullPath = string.Format("{0}/Views/Shared/{1}.cshtml", webProjectPath, partialViewName);

                if (File.Exists(fullPath))
                    return new ViewEngineResult(new TestingView(), this);

                return new ViewEngineResult(this.ViewLocationFormats);
            }
        }

        [Test, TestCaseSource(typeof(ViewConventionTests), "GetEditors")]
        public void EnsureViewForEachEditorExists(Type type)
        {
            string path = type.Namespace.Replace("SAHL.Core.UI.", "");
            path = path.Replace(".", "/");
            path = "EditorTemplates/" + path;
            path += "/Pages/" + type.Name + "Page1Model";

            TestingViewEngine engine = new TestingViewEngine();
            var viewResult = engine.FindPartialView(null, path, false);

            Assert.IsNotNull(viewResult.View, String.Format("TileModel {0} does not have a view. Expected a view in ~/Views/Shared/{1}", type.Name, path));
        }
        public List<Type> GetEditors()
        {
            return GetEditorsByType(typeof(IEditor));
        }

        private List<Type> GetEditorsByType(Type type)
        {
            Assembly sahl_core_ui_halo_assembly = Assembly.GetAssembly(typeof(CreateHelpDeskCaseEditorPage1Model));
            Type[] allTypes = sahl_core_ui_halo_assembly.GetTypes();
            List<Type> editorsTypeDefinitions = new List<Type>();

            var editors = sahl_core_ui_halo_assembly.GetTypes().Where(x =>
                   x.GetInterfaces().Any(y => y.Equals(type)
                   && x.IsAbstract == false) && !typeof(IEditorPageModel).IsAssignableFrom(x)
               ).ToList();

            foreach (var editor in editors)
            {
                editorsTypeDefinitions.Add(editor);
            }
            return editorsTypeDefinitions;
        }

        public class TestingView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {
            }
        }

        public List<Type> GetTileModels()
        {
            return GetTileModelByType(typeof(ITileModel));
        }

        private List<Type> GetTileModelByType(Type type)
        {
            Assembly sahl_core_ui_halo_assembly = Assembly.GetAssembly(typeof(LegalEntityMajorTileContentProvider));
            Type[] allTypes = sahl_core_ui_halo_assembly.GetTypes();
            List<Type> tileModelTypeDefinitions = new List<Type>();
            var tileModels = sahl_core_ui_halo_assembly.GetTypes().Where(x =>
                   x.GetInterfaces().Any(y => y.Equals(type)
                   && x.IsAbstract == false) && !typeof(IActionTileModel).IsAssignableFrom(x)
               ).ToList();
            foreach (var tileModel in tileModels)
            {
                tileModelTypeDefinitions.Add(tileModel);
            }
            return tileModelTypeDefinitions;
        }
    }
}
