using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetApplicationWithoutComcorpOfferPropertyDetailsQuery : ServiceQuery<OfferDataModel>, IFrontEndTestQuery, ISqlServiceQuery<OfferDataModel>
    {
    }
}