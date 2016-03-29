using SAHL.VSExtensions.Configuration.EditorConfiguration.Common;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditor
{
    public class AddEditorConfiguration : EditorConfigurationGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Editor"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ItemPath.Contains("\\Editors\\") && projectItem.ItemPath.Contains("\\Models\\") && projectItem.ProjectName.StartsWith("SAHL.Core.UI.Halo");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            ISAHLProjectItem item = projectItem;
            if (model.CreateFolder)
            {
                item = projectItem.GetOrAddFolder(model.EditorName);
                model.Namespace = item.Namespace;
            }
            ISAHLProjectItem pages = item.GetOrAddFolder("Pages");

            AddEditorTemplate template = new AddEditorTemplate(model);
            item.AddFile(model.EditorName, "cs", template.TransformText());

            foreach (EditorModelInfo info in model.Models)
            {
                string modelName = string.Format("{0}PageModel", info.Name);
                string modelValidator = string.Format("{0}ModelValidator", info.Name);

                PageModelTemplate pageModel = new PageModelTemplate(new { Namespace = pages.Namespace, Name = modelName, UseSQL = info.UseSQL, ParentNamespace = item.Namespace });
                pages.AddFile(modelName, "cs", pageModel.TransformText());

                ModelValidatorTemplate pageValidationModel = new ModelValidatorTemplate(new { Namespace = pages.Namespace, Name = modelValidator, UseSQL = info.UseSQL, PageModelName = modelName });
                pages.AddFile(modelValidator, "cs", pageValidationModel.TransformText());
            }

            if (model.UseUnorderedSelector)
            {
                UnorderedPageSelectorTemplate selectorTemplate = new UnorderedPageSelectorTemplate(model);
                item.AddFile(string.Format("{0}PageSelector", model.EditorName), "cs", selectorTemplate.TransformText());
            }
        }
    }
}