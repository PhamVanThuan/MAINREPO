using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModelView
{
    public class AddTileModelViewConfiguration : TileModelGroup, ISAHLConfiguration
    {
        public string Name
        {
            get { return "Add Tile Model View"; }
        }

        public void Execute(Interfaces.ISAHLProjectItem projectItem, dynamic model)
        {
            AddTileModelViewTemplate template = new AddTileModelViewTemplate(model);
            projectItem.AddFile(model.TileModelName, "cshtml", template.TransformText());
        }

        public bool CanExecute(Interfaces.ISAHLProjectItem projectItem)
        {
            return projectItem.ProjectName.StartsWith("SAHL.Website.Halo");
        }
    }
}