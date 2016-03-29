namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddRootTile
{
    public partial class AddRootTileTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public AddRootTileTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}