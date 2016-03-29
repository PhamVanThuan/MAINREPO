using SAHL.Core.Services;
using SAHL.Services.Interfaces.UserProfile.Models;

namespace SAHL.Services.Interfaces.UserProfile.Queries
{
    public class GetCapabilitiesForUserQuery : ServiceQuery<GetCapabilitiesForUserQueryResult>
    {
        public string ADUsername { get; protected set; }

        public GetCapabilitiesForUserQuery(string adUsername)
        {
            this.ADUsername = adUsername;
        }
    }
}