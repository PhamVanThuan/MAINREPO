namespace SAHL.VSExtensions.Configuration.EditorConfiguration.Common
{
    public partial class PageModelTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public PageModelTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}