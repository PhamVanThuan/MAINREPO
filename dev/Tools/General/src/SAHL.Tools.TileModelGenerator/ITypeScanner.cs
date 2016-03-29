using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public interface ITypeScanner
    {
        IEnumerable<IResult> Scan(string inputLocation);
    }
}
