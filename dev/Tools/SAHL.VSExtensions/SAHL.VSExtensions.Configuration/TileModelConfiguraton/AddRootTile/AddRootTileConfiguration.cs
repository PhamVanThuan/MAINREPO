using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddRootTile
{
    public class AddRootTileConfiguration : TileModelGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Root Tile Config"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ProjectName == "SAHL.Core.UI.Halo" && projectItem.ItemPath.Contains("\\Configuration\\") && projectItem.ItemPath.Contains("\\Tiles\\") && !projectItem.ItemPath.Contains("\\Actions\\");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            AddRootTileTemplate template = new AddRootTileTemplate(model);
            projectItem.AddFile(model.ConfigurationName, "cs", template.TransformText());
        }
    }
}