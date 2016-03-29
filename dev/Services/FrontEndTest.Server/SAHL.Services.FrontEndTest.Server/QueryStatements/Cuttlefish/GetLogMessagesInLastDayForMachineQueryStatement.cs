using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish;
using SAHL.Services.Interfaces.FrontEndTest.Queries.Cuttlefish;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements.Cuttlefish
{
    public class GetLogMessagesInLastDayForMachineQueryStatement : IServiceQuerySqlStatement<GetLogMessagesInLastDayForMachineQuery, GetLogMessagesInLastDayForMachineQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 200 Id, MessageDate, Application, LogMessageType, MethodName, Message, Source, UserName
                    from Cuttlefish.dbo.LogMessage lm
                    where datediff(dd, messageDate, getdate() ) <= 1
                    and LogMessageType <> 'Startup'
                    and MachineName = @MachineName order by 1 desc";
        }
    }
}