using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;

namespace SAHL.Services.Interfaces.Search.Queries
{
    public class GetThirdPartySearchDetailQuery : ServiceQuery<GetThirdPartySearchDetailQueryResult>, ISearchServiceQuery, ISqlServiceQuery<GetThirdPartySearchDetailQueryResult>
    {
        public GetThirdPartySearchDetailQuery(int legalEntityKey)
        {
            this.LegalEntityKey = legalEntityKey;
        }

        public int LegalEntityKey { get; protected set; }
    }
}