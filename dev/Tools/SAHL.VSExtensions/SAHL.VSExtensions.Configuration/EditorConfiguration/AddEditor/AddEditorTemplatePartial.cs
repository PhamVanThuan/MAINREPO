namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditor
{
    public partial class AddEditorTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public AddEditorTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}