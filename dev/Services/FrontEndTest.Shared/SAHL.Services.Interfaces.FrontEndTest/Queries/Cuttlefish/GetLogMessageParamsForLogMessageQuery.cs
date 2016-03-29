using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries.Cuttlefish
{
    public class GetLogMessageParamsForLogMessageQuery : ServiceQuery<GetLogMessageParamsForLogMessageQueryResult>, IFrontEndTestQuery,
        ISqlServiceQuery<GetLogMessageParamsForLogMessageQueryResult>
    {
        public int LogMessage_id { get; set; }

        public GetLogMessageParamsForLogMessageQuery(int logMessage_id)
        {
            this.LogMessage_id = logMessage_id;
        }
    }
}