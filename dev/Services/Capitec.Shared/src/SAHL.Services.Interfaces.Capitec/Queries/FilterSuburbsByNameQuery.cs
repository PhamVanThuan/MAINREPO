using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class FilterSuburbsByNameQuery : ServiceQuery<FilterSuburbsByNameQueryResult>, ISqlServiceQuery<FilterSuburbsByNameQueryResult>
    {
        public string SuburbNameFilter { get; protected set; }

        public FilterSuburbsByNameQuery(string suburbNameFilter)
        {
            this.SuburbNameFilter = suburbNameFilter;
        }
    }
}