using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.ActionConfiguration.AddActionTileModel
{
    public class AddActionTileModelConfiguration : ActionConfigurationGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Action Tile Model"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ItemPath.Contains("\\Actions\\") && projectItem.ItemPath.Contains("\\Models\\") && projectItem.ProjectName.StartsWith("SAHL.Core.UI.Halo");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            AddActionTileModelTemplate template = new AddActionTileModelTemplate(model);
            projectItem.AddFile(model.ActionTileModelName, "cs", template.TransformText());
        }
    }
}