using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.UserProfile.Queries;
using SAHL.Services.UserProfile.QueryHandlers;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.UserProfile.Specs.QueryHandlers.GetCapabilitiesForUserOrganisationStructureQueryHandlerSpecs
{
    [Subject("SAHL.Services.UserProfile.QueryHandlers.GetCapabilitiesForUserOrganisationStructureQueryHandler.Handle")]
    public class when_invalid_user_organisation_structure_key_queried : WithFakes
    {
        private static IUserManager userManager;
        private static GetCapabilitiesForUserOrganisationStructureQuery query;
        private static GetCapabilitiesForUserOrganisationStructureQueryHandler queryHandler;
        private static int userOrganisationStructureKey;

        private Establish context = () =>
        {
            userOrganisationStructureKey = -1;
            query = new GetCapabilitiesForUserOrganisationStructureQuery(userOrganisationStructureKey);
            userManager = An<IUserManager>();

            List<string> capabilities = new List<string>();

            userManager.WhenToldTo(x => x.GetUserCapabilitiesForOrganisationStructureKey(userOrganisationStructureKey)).Return(capabilities);

            queryHandler = new GetCapabilitiesForUserOrganisationStructureQueryHandler(userManager);
        };

        private Because of = () =>
        {
            queryHandler.HandleQuery(query);
        };

        private It should_not_get_any_capabillity = () =>
        {
            query.Result.Results.All(r => r.Capabilities.Count() == 0).ShouldBeTrue();
        };
    }
}