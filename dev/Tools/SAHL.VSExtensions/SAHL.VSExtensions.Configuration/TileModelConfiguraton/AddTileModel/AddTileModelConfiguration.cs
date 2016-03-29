using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModel
{
    public class AddTileModelConfiguration : TileModelGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Tile Model"; }
        }

        public void Execute(ISAHLProjectItem projectItem, dynamic model)
        {
            TileModelTemplate tileModel = new TileModelTemplate(model);
            TileContentProviderTemplate content = new TileContentProviderTemplate(model);
            TileDataProviderTemplate data = new TileDataProviderTemplate(model);

            projectItem.AddFile(model.ModelName, "cs", tileModel.TransformText());
            projectItem.AddFile(model.ContentProvider, "cs", content.TransformText());
            projectItem.AddFile(model.DataProvider, "cs", data.TransformText());
        }

        public bool CanExecute(ISAHLProjectItem projectItem)
        {
            return projectItem.ItemPath.Contains("\\Tiles\\") && projectItem.ItemPath.Contains("\\Models\\") && projectItem.ProjectName.StartsWith("SAHL.Core.UI.Halo");
        }
    }
}