using SAHL.VSExtensions.Configuration.EditorConfiguration.Common;
using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditorModel
{
    public class AddEditorModelConfiguration : EditorConfigurationGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Editor Model"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ItemPath.Contains("\\Editors\\") && projectItem.ItemPath.Contains("\\Models\\") && projectItem.ItemPath.Contains("\\Pages\\") && projectItem.ProjectName.StartsWith("SAHL.Core.UI.Halo");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            string modelName = string.Format("{0}PageModel", model.Name);
            string modelValidator = string.Format("{0}ModelValidator", model.Name);

            PageModelTemplate pageModel = new PageModelTemplate(new { Namespace = projectItem.Namespace, Name = modelName, UseSQL = model.UseSQL, ParentNamespace = model.Namespace });
            projectItem.AddFile(modelName, "cs", pageModel.TransformText());

            ModelValidatorTemplate pageValidationModel = new ModelValidatorTemplate(new { Namespace = projectItem.Namespace, Name = modelValidator, UseSQL = model.UseSQL, PageModelName = modelName });
            projectItem.AddFile(modelValidator, "cs", pageValidationModel.TransformText());
        }
    }
}