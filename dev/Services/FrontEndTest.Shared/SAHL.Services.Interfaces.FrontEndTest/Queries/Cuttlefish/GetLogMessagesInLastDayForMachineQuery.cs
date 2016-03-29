using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries.Cuttlefish
{
    public class GetLogMessagesInLastDayForMachineQuery : ServiceQuery<GetLogMessagesInLastDayForMachineQueryResult>, IFrontEndTestQuery,
        ISqlServiceQuery<GetLogMessagesInLastDayForMachineQueryResult>
    {
        public string MachineName { get; set; }

        public GetLogMessagesInLastDayForMachineQuery(string machineName)
        {
            this.MachineName = machineName;
        }
    }
}