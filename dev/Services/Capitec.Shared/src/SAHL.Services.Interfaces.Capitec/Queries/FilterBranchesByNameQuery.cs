using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class FilterBranchesByNameQuery : ServiceQuery<FilterBranchesByNameQueryResult>, ISqlServiceQuery<FilterBranchesByNameQueryResult>
    {
        public string BranchName { get; protected set; }

        public FilterBranchesByNameQuery(string branchName)
        {
            this.BranchName = branchName;
        }
    }
}