using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetDisabilityClaimQuery : ServiceQuery<DisabilityClaimDataModel>, IFrontEndTestQuery, ISqlServiceQuery<DisabilityClaimDataModel>
    {
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimQuery(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}