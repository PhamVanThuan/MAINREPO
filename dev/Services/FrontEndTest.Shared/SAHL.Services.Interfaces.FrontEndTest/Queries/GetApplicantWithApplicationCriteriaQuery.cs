using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetApplicantWithApplicationCriteriaQuery : ServiceQuery<GetApplicantWithApplicationCriteriaQueryResult>, IFrontEndTestQuery,
        ISqlServiceQuery<GetApplicantWithApplicationCriteriaQueryResult>
    {
        public bool ApplicantHasAddress { get; protected set; }

        public bool ApplicationHasMailingAddress { get; protected set; }

        public GetApplicantWithApplicationCriteriaQuery(bool applicantHasAddress, bool applicationHasMailingAddress)
        {
            this.ApplicantHasAddress = applicantHasAddress;
            this.ApplicationHasMailingAddress = applicationHasMailingAddress;
        }
    }
}