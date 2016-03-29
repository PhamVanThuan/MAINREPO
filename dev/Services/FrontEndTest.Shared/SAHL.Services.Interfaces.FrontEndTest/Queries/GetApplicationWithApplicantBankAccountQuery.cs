using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetApplicationWithApplicantBankAccountQuery : ServiceQuery<GetApplicationWithApplicantBankAccountQueryResult>, IFrontEndTestQuery,
        ISqlServiceQuery<GetApplicationWithApplicantBankAccountQueryResult>
    {
        public GetApplicationWithApplicantBankAccountQuery(bool hasDebitOrder)
        {
            this.HasDebitOrder = hasDebitOrder;
        }

        public bool HasDebitOrder { get; protected set; }
    }
}