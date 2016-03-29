using SAHL.Core.Services;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.Interfaces.ITC.Queries
{
    public class GetCurrentITCForLegalEntityQuery : ServiceQuery<GetCurrentITCForLegalEntityQueryResult>, IITCServiceQuery
    {
        public string IdentityNumber { get; protected set; }

        public GetCurrentITCForLegalEntityQuery(string identityNumber)
        {
            this.IdentityNumber = identityNumber;
        }
    }
}