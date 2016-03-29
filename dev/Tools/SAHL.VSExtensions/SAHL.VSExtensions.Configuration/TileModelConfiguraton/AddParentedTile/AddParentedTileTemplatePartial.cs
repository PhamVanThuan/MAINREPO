namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddParentedTile
{
    public partial class AddParentedTileTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public AddParentedTileTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}