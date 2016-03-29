using Mono.Cecil;
using SAHL.Tools.TileModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Reflection
{
    public class TileEditorConvention : IScanConvention
    {
        public IList<TypeDefinition> Result { get; protected set; }

        public TileEditorConvention()
        {
            this.Result = new List<TypeDefinition>();
        }

        public void ProcessType(TypeDefinition definition)
        {
            if (definition.IsClass && definition.IsPublic && definition.HasInterfaces && definition.Interfaces.Any(x => x.Name == "IHaloTileEditorConfiguration"))
            {
                this.Result.Add(definition);
            }
        }
    }
}
