using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Models
{
    public class GlobalsModel
    {
        public TypeDefinition TypeDefinition { get; protected set; }
        public IEnumerable<FieldDefinition> ActualModel { get; protected set; }

        public GlobalsModel(TypeDefinition typeDefinition)
        {
            this.TypeDefinition = typeDefinition;
            this.ActualModel = typeDefinition.Fields.AsEnumerable();
        }
    }
}
