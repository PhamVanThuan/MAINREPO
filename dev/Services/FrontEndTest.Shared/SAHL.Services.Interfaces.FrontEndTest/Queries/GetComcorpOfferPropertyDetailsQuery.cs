using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetComcorpOfferPropertyDetailsQuery : ServiceQuery<ComcorpOfferPropertyDetailsDataModel>, IFrontEndTestQuery
                                                      ,ISqlServiceQuery<ComcorpOfferPropertyDetailsDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public GetComcorpOfferPropertyDetailsQuery(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }
    }
}