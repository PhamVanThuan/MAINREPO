using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetLatestApplicationInformationVariableLoanQuery : ServiceQuery<GetLatestApplicationInformationVariableLoanQueryResult>, IFrontEndTestQuery
                                                                   ,ISqlServiceQuery<GetLatestApplicationInformationVariableLoanQueryResult>
    {
        public int ApplicationNumber { get; protected set; }

        public GetLatestApplicationInformationVariableLoanQuery(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }
    }
}