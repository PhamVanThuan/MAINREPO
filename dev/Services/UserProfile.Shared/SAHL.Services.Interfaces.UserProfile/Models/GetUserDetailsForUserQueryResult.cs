using SAHL.Services.Interfaces.UserProfile.Models.Shared;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.UserProfile.Models
{
    public class GetUserDetailsForUserQueryResult
    {
        public string UserName { get; protected set; }

        public string Domain { get; protected set; }

        public string DisplayName { get; protected set; }

        public string EmailAddress { get; protected set; }

        public IEnumerable<UserAreaRole> Roles { get; protected set; }

        public GetUserDetailsForUserQueryResult(string username, string domain, string displayName, string emailAddress, IEnumerable<UserAreaRole> roles)
        {
            this.UserName = username;
            this.Domain = domain;
            this.DisplayName = displayName;
            this.EmailAddress = emailAddress;
            this.Roles = roles;
        }
    }
}