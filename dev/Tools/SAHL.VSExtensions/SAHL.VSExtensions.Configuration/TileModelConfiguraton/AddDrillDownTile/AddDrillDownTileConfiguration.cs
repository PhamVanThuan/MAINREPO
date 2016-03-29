using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddDrillDownTile
{
    public class AddDrillDownTileConfiguration : TileModelGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Drill Down Tile Config"; }
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ProjectName == "SAHL.Core.UI.Halo" && projectItem.ItemPath.Contains("\\Configuration\\") && projectItem.ItemPath.Contains("\\Tiles\\") && !projectItem.ItemPath.Contains("\\Actions\\");
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            AddDrillDownTileTemplate template = new AddDrillDownTileTemplate(model);
            projectItem.AddFile(model.ConfigurationName, "cs", template.TransformText());
        }
    }
}