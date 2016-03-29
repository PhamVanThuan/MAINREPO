namespace SAHL.VSExtensions.Configuration.EditorConfiguration.Common
{
    public partial class ModelValidatorTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public ModelValidatorTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}