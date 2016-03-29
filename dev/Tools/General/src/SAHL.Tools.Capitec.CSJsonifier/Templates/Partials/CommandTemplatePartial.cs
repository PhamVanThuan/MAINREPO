using SAHL.Tools.Capitec.CSJsonifier.Reflection;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Partials
{
    public partial class CommandTemplate : ITemplate, ITemplateForModel<CommandScanConvention>
    {
        internal ITemplateManager TemplateManager { get; set; }

        public CommandScanConvention Model { get; protected set; }

        public CommandTemplate(ITemplateManager templateManager)
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