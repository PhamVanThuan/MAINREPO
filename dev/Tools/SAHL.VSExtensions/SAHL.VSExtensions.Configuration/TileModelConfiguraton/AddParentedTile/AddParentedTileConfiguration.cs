using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddParentedTile
{
    public class AddParentedTileConfiguration : TileModelGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Parented Tile Config"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ProjectName == "SAHL.Core.UI.Halo" && projectItem.ItemPath.Contains("\\Configuration\\") && projectItem.ItemPath.Contains("\\Tiles\\") && !projectItem.ItemPath.Contains("\\Actions\\");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            AddParentedTileTemplate template = new AddParentedTileTemplate(model);
            projectItem.AddFile(model.ConfigurationName, "cs", template.TransformText());
        }
    }
}