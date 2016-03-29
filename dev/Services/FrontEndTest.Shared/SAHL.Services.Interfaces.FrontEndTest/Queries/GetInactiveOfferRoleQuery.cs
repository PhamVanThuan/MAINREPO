using SAHL.Core.Data;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetInactiveOfferRoleQuery : ServiceQuery<int>, IFrontEndTestQuery, ISqlServiceQuery<int>
    {
    }
}