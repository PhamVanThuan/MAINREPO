using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish;
using SAHL.Services.Interfaces.FrontEndTest.Queries.Cuttlefish;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements.Cuttlefish
{
    public class GetLogMessageParamsForLogMessageQueryStatement : IServiceQuerySqlStatement<GetLogMessageParamsForLogMessageQuery, GetLogMessageParamsForLogMessageQueryResult>
    {
        public string GetStatement()
        {
            return @"select ParameterValue, mp.ParameterKey
                    from Cuttlefish.dbo.MessageParameters mp
                    where LogMessage_id = @logmessage_id";
        }
    }
}