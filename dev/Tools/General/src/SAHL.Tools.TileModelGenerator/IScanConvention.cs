using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public interface IScanConvention : IResult
    {
        void ProcessType(TypeDefinition definition);
    }
}
