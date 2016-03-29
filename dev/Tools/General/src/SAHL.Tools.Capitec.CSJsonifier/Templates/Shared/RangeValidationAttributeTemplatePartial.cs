using SAHL.Tools.Capitec.CSJsonifier.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Shared
{
    public partial class RangeValidationAttributeTemplate : ITemplate, ITemplateForModel<RangeValidationAttribute>
    {
        public RangeValidationAttribute Model { get; protected set; }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }
    }
}
