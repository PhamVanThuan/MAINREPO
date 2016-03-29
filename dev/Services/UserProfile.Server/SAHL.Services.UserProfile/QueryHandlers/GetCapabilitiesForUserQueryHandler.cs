using SAHL.Core.Identity;
using SAHL.Core.Identity.Model;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.UserProfile.Models;
using SAHL.Services.Interfaces.UserProfile.Models.Shared;
using SAHL.Services.Interfaces.UserProfile.Queries;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.UserProfile.QueryHandlers
{
    public class GetCapabilitiesForUserQueryHandler : IServiceQueryHandler<GetCapabilitiesForUserQuery>
    {
        private IUserManager userManager;

        public GetCapabilitiesForUserQueryHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public ISystemMessageCollection HandleQuery(GetCapabilitiesForUserQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();
            IEnumerable<OrganisationStructureCapability> capabilities = userManager.GetUserCapabilities(query.ADUsername);
            var results = capabilities.GroupBy(x => x.UserOrganisationStructureKey)
                         .Select(y => new RoleCapabilities(y.Key, y.Select(z=>z.CapabilityDescription)));
            query.Result = new ServiceQueryResult<GetCapabilitiesForUserQueryResult>(new GetCapabilitiesForUserQueryResult[]{
                new GetCapabilitiesForUserQueryResult(results)
            });
            return systemMessages;
        }
    }
}
