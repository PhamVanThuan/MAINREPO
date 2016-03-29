namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModel
{
    public partial class TileModelTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public TileModelTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}