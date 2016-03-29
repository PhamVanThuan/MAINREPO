using SAHL.Tools.Capitec.CSJsonifier.Models;
using SAHL.Tools.Capitec.CSJsonifier.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Partials
{
    public partial class SharedModelsTemplate : ITemplate, ITemplateForModel<SharedModels>
    {
        internal ITemplateManager TemplateManager { get; set; }

        public SharedModels Model { get; protected set; }

        public SharedModelsTemplate(ITemplateManager templateManager)
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
