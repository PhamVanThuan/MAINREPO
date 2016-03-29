using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.UserProfile.Models;
using SAHL.Services.Interfaces.UserProfile.Queries;
using System;

namespace SAHL.Services.UserProfile.QueryHandlers
{
    public class GetCapabilitiesForUserOrganisationStructureQueryHandler : IServiceQueryHandler<GetCapabilitiesForUserOrganisationStructureQuery>
    {
        private IUserManager userManager;

        public GetCapabilitiesForUserOrganisationStructureQueryHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public ISystemMessageCollection HandleQuery(GetCapabilitiesForUserOrganisationStructureQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();

            try
            {
                var capabilities = userManager.GetUserCapabilitiesForOrganisationStructureKey(query.UserOrganisationStructureKey);

                query.Result = new ServiceQueryResult<GetCapabilitiesForUserOrganisationStructureQueryResult>(
                    new GetCapabilitiesForUserOrganisationStructureQueryResult[] { new GetCapabilitiesForUserOrganisationStructureQueryResult(capabilities) });
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("Failed to retrieve capabilities for user organisation structure key {0}, Exception: {1}"
                    , query.UserOrganisationStructureKey, ex);

                systemMessages.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Exception));
            }

            return systemMessages;
        }
    }
}