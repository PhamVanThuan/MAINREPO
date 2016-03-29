using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries.Cuttlefish
{
    public class GetMachinesWithLogMessagesInLastDayQuery : ServiceQuery<GetMachinesWithLogMessagesInLastDayQueryResult>, IFrontEndTestQuery, 
        ISqlServiceQuery<GetMachinesWithLogMessagesInLastDayQueryResult>
    {
    }
}
