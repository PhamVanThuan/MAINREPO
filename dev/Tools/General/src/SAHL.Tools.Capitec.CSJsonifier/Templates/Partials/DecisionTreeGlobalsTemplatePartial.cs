using SAHL.Tools.Capitec.CSJsonifier.Filters;
using SAHL.Tools.Capitec.CSJsonifier.Models;
using SAHL.Tools.Capitec.CSJsonifier.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Partials
{
    public partial class DecisionTreeGlobalsTemplate : ITemplate, ITemplateForModel<DecisionTreeGlobalsConvention>
    {
        internal ITemplateManager TemplateManager { get; set; }

        public IEnumerable<GlobalsModel> Filter
        {
            get;
            protected set;
        }
        
        public DecisionTreeGlobalsConvention Model
        {
            get;
            protected set;
        }

        public DecisionTreeGlobalsTemplate(ITemplateManager templateManager)
        {
            this.TemplateManager = templateManager;
        }

        public string Process(dynamic model)
        {
            this.Model = model;
            this.Filter = new DecisionTreeGlobalsFilter().Filter(model);
            return this.TransformText();
        }
    }
}
