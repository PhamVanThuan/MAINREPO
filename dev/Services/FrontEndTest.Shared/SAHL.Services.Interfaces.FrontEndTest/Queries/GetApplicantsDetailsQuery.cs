using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetApplicantsDetailsQuery : ServiceQuery<LegalEntityDataModel>, IFrontEndTestQuery, ISqlServiceQuery<LegalEntityDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public GetApplicantsDetailsQuery(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }
    }
}