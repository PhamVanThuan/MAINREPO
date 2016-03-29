using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using System;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditorConfig
{
    public class AddEditorConfiguration : EditorConfigurationGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Editor Configuration"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ProjectName == "SAHL.Core.UI.Halo" && projectItem.ItemPath.Contains("\\Configuration\\") && projectItem.ItemPath.Contains("\\Editors\\");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            EditorConfigurationTemplate configTemplate = new EditorConfigurationTemplate(model);
            projectItem.AddFile(model.EditorName, "cs", configTemplate.TransformText());

            Interfaces.ISAHLProjectItem pages = projectItem.GetOrAddFolder("Pages");
            int i = 1;
            foreach (ITypeInfo typeInfo in model.Pages)
            {
                string pageName = model.IsOrderedSelector ? string.Format("{0}Page{1}Configuration", model.EditorShortName, i) : typeInfo.Name.Replace("Model","Configuration");
                EditorPageConfigurationTemplate pageTemplate = new EditorPageConfigurationTemplate(new { Namespace = pages.Namespace, PageName = pageName, IsOrderedSelector = model.IsOrderedSelector, Postion = i,PageModel = typeInfo.FullName,EditorName = model.EditorName});
                pages.AddFile(pageName, "cs", pageTemplate.TransformText());
                i++;
            }
            Interfaces.ISAHLProjectItem selector = pages.GetOrAddFolder("Selector");
            EditorSelectorTemplate selectorTemplate = new EditorSelectorTemplate(new { Namespace = selector.Namespace, SelectorName = model.SelectorName, IsOrderedSelector = model.IsOrderedSelector, EditorName = model.EditorName, CustomPageSelector = model.CustomPageSelector });
            selector.AddFile(model.SelectorName, "cs", selectorTemplate.TransformText());
        }
    }
}