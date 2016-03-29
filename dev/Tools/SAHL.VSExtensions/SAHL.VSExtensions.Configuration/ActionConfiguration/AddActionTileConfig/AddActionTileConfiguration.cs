using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.ActionConfiguration.AddActionTileConfig
{
    public class AddActionTileConfiguration : ActionConfigurationGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Action Tile Configuration"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ProjectName == "SAHL.Core.UI.Halo" && projectItem.ItemPath.Contains("\\Configuration\\") && projectItem.ItemPath.Contains("\\Actions\\");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            AddActionTileTemplate template = new AddActionTileTemplate(model);
            projectItem.AddFile(model.ActionConfigurationName, "cs", template.TransformText());
        }
    }
}