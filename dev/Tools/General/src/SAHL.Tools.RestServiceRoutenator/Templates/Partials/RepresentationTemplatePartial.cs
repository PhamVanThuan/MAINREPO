using SAHL.Tools.RestServiceRoutenator.Reflection;

namespace SAHL.Tools.RestServiceRoutenator.Templates.Partials
{
    public partial class RepresentationTemplate : ITemplate, ITemplateForModel<MonoCecilRepresentationRestQueryScan>
    {
        private readonly ITemplateManager TemplateManager;

        public RepresentationTemplate(ITemplateManager manager)
        {
            this.TemplateManager = manager;
        }

        public string Process(dynamic model)
        {
            Model = model;
            return this.TransformText();
        }

        public MonoCecilRepresentationRestQueryScan Model { get; private set; }
    }
}