namespace SAHL.VSExtensions.Configuration.ActionConfiguration.AddActionTileModel
{
    public partial class AddActionTileModelTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public AddActionTileModelTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}