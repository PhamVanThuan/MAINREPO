using SAHL.Tools.Capitec.CSJsonifier.Reflection;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Partials
{
    public partial class CommandQueryTemplate : ITemplate, ITemplateForModel<QueryScanConvention>
    {
        internal ITemplateManager TemplateManager { get; set; }
        internal ISharedModelManager DataModelManager { get; set; }

        public QueryScanConvention Model
        {
            get;
            protected set;
        }

        public CommandQueryTemplate(ITemplateManager templateManager,ISharedModelManager dataModelManager)
        {
            this.TemplateManager = templateManager;
            this.DataModelManager = dataModelManager;
        }

        public string Process(dynamic model)
        {
            this.Model = model;
            
            return this.TransformText();
        }
    }
}