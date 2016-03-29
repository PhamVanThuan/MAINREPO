using SAHL.Tools.Capitec.CSJsonifier.Reflection;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Partials
{
    public partial class SearchQueryTemplate : ITemplate, ITemplateForModel<SearchQueryScanConvention>
    {
        internal ITemplateManager TemplateManager { get; set; }

        public SearchQueryScanConvention Model
        {
            get;
            protected set;
        }

        public SearchQueryTemplate(ITemplateManager templateManager)
        {
            this.TemplateManager = templateManager;
        }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }
    }
}