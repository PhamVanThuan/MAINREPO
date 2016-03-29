using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface IScanConventionFilter<T,O> where T : IScanConvention
    {
        IEnumerable<O> Filter(T model);
    }
}
