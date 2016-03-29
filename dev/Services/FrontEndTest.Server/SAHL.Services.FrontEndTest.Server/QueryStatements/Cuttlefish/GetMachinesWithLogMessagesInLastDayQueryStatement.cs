using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish;
using SAHL.Services.Interfaces.FrontEndTest.Queries.Cuttlefish;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements.Cuttlefish
{
    public class GetMachinesWithLogMessagesInLastDayQueryStatement : IServiceQuerySqlStatement<GetMachinesWithLogMessagesInLastDayQuery, GetMachinesWithLogMessagesInLastDayQueryResult>
    {
        public string GetStatement()
        {
            return @"select MachineName, count(Id) as ErrorCount
                from Cuttlefish.dbo.LogMessage
                where datediff(dd, messageDate, getdate() ) <= 1
                    and LogMessageType <> 'Startup'
                group by MachineName";
        }
    }
}