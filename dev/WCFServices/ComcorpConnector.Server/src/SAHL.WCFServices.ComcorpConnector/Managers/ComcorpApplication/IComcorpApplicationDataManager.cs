using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication
{
    public interface IComcorpApplicationDataManager
    {
        int? GetApplicationNumberForApplicationCode(long applicationCode);
    }
}