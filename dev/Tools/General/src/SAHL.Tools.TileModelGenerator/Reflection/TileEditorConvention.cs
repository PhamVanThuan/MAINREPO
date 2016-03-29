using Mono.Cecil;
using SAHL.Tools.TileModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Reflection
{
    public class TileModelConvention : IScanConvention
    {
        public IList<TypeDefinition> Result { get; protected set; }

        public TileModelConvention(){
            this.Result = new List<TypeDefinition>();
        }

        public void ProcessType(TypeDefinition definition)
        {
            if (definition.IsClass && definition.IsPublic && definition.HasInterfaces && definition.Interfaces.Any(x => x.Name == "IHaloTileModel"))
            {
                this.Result.Add(definition);
            }
        }
    }
}
