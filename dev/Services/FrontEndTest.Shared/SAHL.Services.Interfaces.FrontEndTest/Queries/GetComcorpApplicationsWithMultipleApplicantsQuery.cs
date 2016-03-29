using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetComcorpApplicationsWithMultipleApplicantsQuery : ServiceQuery<GetComcorpApplicationsWithMultipleApplicantsQueryResult>, IFrontEndTestQuery,
                                                                     ISqlServiceQuery<GetComcorpApplicationsWithMultipleApplicantsQueryResult>
    {
    }
}