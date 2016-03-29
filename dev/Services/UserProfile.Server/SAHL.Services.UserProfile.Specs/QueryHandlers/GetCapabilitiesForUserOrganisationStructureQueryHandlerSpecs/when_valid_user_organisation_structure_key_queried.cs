using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.UserProfile.Queries;
using SAHL.Services.UserProfile.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.UserProfile.Specs.QueryHandlers.GetCapabilitiesForUserOrganisationStructureQueryHandlerSpecs
{
     [Subject("SAHL.Services.UserProfile.QueryHandlers.GetCapabilitiesForUserOrganisationStructureQueryHandler.Handle")]
    public class when_valid_user_organisation_structure_key_queried : WithFakes
    {
        static IUserManager userManager;
        static GetCapabilitiesForUserOrganisationStructureQuery query;
        static GetCapabilitiesForUserOrganisationStructureQueryHandler queryHandler;
        static int userOrganisationStructureKey;

        Establish context = () =>
        {
            userOrganisationStructureKey = 6980;
            query = new GetCapabilitiesForUserOrganisationStructureQuery(userOrganisationStructureKey);
            userManager = An<IUserManager>();

            List<string> capabilities = new List<string> { };

            userManager.WhenToldTo(x => x.GetUserCapabilitiesForOrganisationStructureKey(userOrganisationStructureKey)).Return(capabilities);

            queryHandler = new GetCapabilitiesForUserOrganisationStructureQueryHandler(userManager);
        };

        Because of = () =>
        {
            queryHandler.HandleQuery(query);
        };

        It should_set_result = () =>
        {
            query.Result.ShouldNotBeNull();
        };

        It should_get_capabillites_from_user_manager = () =>
        {
            userManager.WasToldTo(x => x.GetUserCapabilitiesForOrganisationStructureKey(userOrganisationStructureKey));
        };
    }
}
