using SAHL.Tools.Capitec.CSJsonifier.Models;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface ISharedModelManager
    {
        IScanResult GetComplexModels(IEnumerable<IScanResult> result);
    }
}
