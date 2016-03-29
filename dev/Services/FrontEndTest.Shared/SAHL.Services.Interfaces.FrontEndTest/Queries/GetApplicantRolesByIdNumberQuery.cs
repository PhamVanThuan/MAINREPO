using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetApplicantRolesByIdNumberQuery : ServiceQuery<OfferRoleDataModel>, IFrontEndTestQuery, ISqlServiceQuery<OfferRoleDataModel>
    {
        public string IdNumber { get; protected set; }

        public GetApplicantRolesByIdNumberQuery(string IdNumber)
        {
            this.IdNumber = IdNumber;
        }
    }
}