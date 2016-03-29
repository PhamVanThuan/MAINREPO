namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModel
{
    public partial class TileDataProviderTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public TileDataProviderTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}