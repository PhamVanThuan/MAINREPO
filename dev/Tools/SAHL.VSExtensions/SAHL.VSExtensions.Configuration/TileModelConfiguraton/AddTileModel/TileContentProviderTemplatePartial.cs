namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModel
{
    public partial class TileContentProviderTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public TileContentProviderTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}