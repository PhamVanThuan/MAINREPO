using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;

using SAHL.Services.Interfaces.UserProfile.Models;
using SAHL.Services.Interfaces.UserProfile.Queries;

namespace SAHL.Websites.Halo.Shared
{
    public class UserDetailsStartupAction : UnitOfWorkActionBase
    {
        private UserProfileHttpServiceClient serviceClient;

        public UserDetailsStartupAction()
        {
            this.Sequence = 1;
            serviceClient = UserProfileHttpServiceClient.Instance();
        }

        public static GetUserDetailsForUserQueryResult UserInfo { get; internal set; }
        public static IEnumerable<string> UserRoles { get; internal set; }
        public static IEnumerable<string> Capabilities { get; internal set; }

        public override bool Execute()
        {
            var query = new GetUserDetailsForUserQuery(this.CurrentUser.Identity.Name);
            serviceClient.PerformQuery(query);

            UserDetailsStartupAction.UserInfo = query.Result.Results.First();
            if ((UserDetailsStartupAction.UserInfo == null) || !UserDetailsStartupAction.UserInfo.Roles.Any()) { return false; }

            UserDetailsStartupAction.UserRoles = UserDetailsStartupAction.UserInfo.Roles.Select(x =>
                                                        String.Format("{{OrganisationArea:'{0}', RoleName: '{1}', UserOrganisationStructureKey: '{2}'}}", 
                                                        x.OrganisationArea, x.RoleName, x.UserOrganisationstructureKey.Value));

            
            int defaultUserOrganisationStructureKey = UserDetailsStartupAction.UserInfo.Roles.First().UserOrganisationstructureKey.Value;

            var capabilitiesQuery = new GetCapabilitiesForUserOrganisationStructureQuery(defaultUserOrganisationStructureKey);

            serviceClient.PerformQuery(capabilitiesQuery);
            
            UserDetailsStartupAction.Capabilities = capabilitiesQuery.Result.Results.First().Capabilities.Count() == 0 
                ? Enumerable.Empty<string>() : capabilitiesQuery.Result.Results.First().Capabilities.Select(x=>string.Format("'{0}'",x));
	 

            return true;
        }
    }
}
