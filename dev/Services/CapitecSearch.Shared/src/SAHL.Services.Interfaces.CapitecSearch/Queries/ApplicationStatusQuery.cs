using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.CapitecSearch.Models;

namespace SAHL.Services.Interfaces.CapitecSearch.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class ApplicationStatusQuery : ServiceQuery<ApplicationStatusQueryResult>, IFullTextServiceQuery
    {
        public string ApplicationNumber { get; set; }

        public string IdentityNumberList { get; set; }

        public ApplicationStatusQuery(string applicationNumber, string identityNumberList)
        {
            this.ApplicationNumber = applicationNumber;
            this.IdentityNumberList = identityNumberList;
        }

        public ApplicationStatusQuery()
        {
        }
    }
}