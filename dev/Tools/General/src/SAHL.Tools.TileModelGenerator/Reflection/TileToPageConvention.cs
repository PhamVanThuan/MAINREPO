using Mono.Cecil;
using Mono.Cecil.Cil;
using SAHL.Tools.TileModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Reflection
{
    public class TileToPageConvention : IScanConvention
    {
        public IList<TypeDefinition> Result { get; protected set; }


        public TileToPageConvention()
        {
            this.Result = new List<TypeDefinition>();
        }

        public void ProcessType(TypeDefinition definition)
        {
            if (definition.IsClass && definition.Interfaces.Any(x=>x.Name == "IHaloTileState"))
            {
                Result.Add(definition);
            }
        }
    }
}
