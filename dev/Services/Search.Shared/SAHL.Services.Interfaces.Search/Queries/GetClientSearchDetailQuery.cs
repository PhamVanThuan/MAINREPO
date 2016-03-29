using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;

namespace SAHL.Services.Interfaces.Search.Queries
{
    public class GetClientSearchDetailQuery : ServiceQuery<GetClientSearchDetailQueryResult>, ISearchServiceQuery, ISqlServiceQuery<GetClientSearchDetailQueryResult>
    {
        public GetClientSearchDetailQuery(int legalEntityKey)
        {
            this.LegalEntityKey = legalEntityKey;
        }

        public int LegalEntityKey { get; protected set; }
    }
}