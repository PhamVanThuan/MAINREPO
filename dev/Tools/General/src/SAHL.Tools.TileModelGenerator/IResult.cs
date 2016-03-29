using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Tools.TileModelGenerator
{
    public interface IResult
    {
        IList<TypeDefinition> Result { get; }
    }
}
