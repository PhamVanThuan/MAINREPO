namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddDrillDownTile
{
    public partial class AddDrillDownTileTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public AddDrillDownTileTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}