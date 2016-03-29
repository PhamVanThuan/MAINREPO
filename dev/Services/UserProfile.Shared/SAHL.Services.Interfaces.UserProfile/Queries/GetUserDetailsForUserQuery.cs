using SAHL.Core.Services;
using SAHL.Services.Interfaces.UserProfile.Models;

namespace SAHL.Services.Interfaces.UserProfile.Queries
{
    public class GetUserDetailsForUserQuery : ServiceQuery<GetUserDetailsForUserQueryResult>
    {
        public string ADUsername { get; protected set; }

        public GetUserDetailsForUserQuery(string adUsername)
        {
            this.ADUsername = adUsername;
        }
    }
}