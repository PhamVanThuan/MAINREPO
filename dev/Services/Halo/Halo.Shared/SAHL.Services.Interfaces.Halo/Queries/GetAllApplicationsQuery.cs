using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.Halo
{
    public class GetAllApplicationsQuery : ServiceQuery<HaloApplicationModel>, IHaloServiceQuery
    {
    }
}
