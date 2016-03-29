using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.UserProfile.Queries;
using SAHL.Services.UserProfile.QueryHandlers;
using System;
using System.Linq;

namespace SAHL.Services.UserProfile.Specs.QueryHandlers.GetCapabilitiesForUserOrganisationStructureQueryHandlerSpecs
{
    [Subject("SAHL.Services.UserProfile.QueryHandlers.GetCapabilitiesForUserOrganisationStructureQueryHandler.Handle")]
    public class when_something_goes_wrong_with_the_infrastructure : WithFakes
    {
        private static IUserManager userManager;
        private static GetCapabilitiesForUserOrganisationStructureQuery query;
        private static GetCapabilitiesForUserOrganisationStructureQueryHandler queryHandler;
        private static int userOrganisationStructureKey;
        private static Exception expectedException;
        private static ISystemMessageCollection systemMessages;
        private static string expectedErrorMessage;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();

            userOrganisationStructureKey = 8475;
            query = new GetCapabilitiesForUserOrganisationStructureQuery(userOrganisationStructureKey);
            userManager = An<IUserManager>();

            var innerException = new Exception("Time out ...");
            userManager.WhenToldTo(x => x.GetUserCapabilitiesForOrganisationStructureKey(userOrganisationStructureKey)).Throw(innerException);
            expectedErrorMessage = string.Format("Failed to retrieve capabilities for user organisation structure key {0}, Exception: {1}"
                    , userOrganisationStructureKey, innerException);

            queryHandler = new GetCapabilitiesForUserOrganisationStructureQueryHandler(userManager);
        };

        private Because of = () =>
        {
            expectedException = Catch.Exception(() => queryHandler.HandleQuery(query));
        };

        private It should_handle_the_bubbled_exception = () =>
        {
            expectedException.ShouldBeNull();
        };

        private It should_not_get_any_result = () =>
        {
            query.Result.ShouldBeNull();
        };

        private It should_return_a_human_friendly_message = () =>
        {
            systemMessages.ErrorMessages().Any(m => m.Message.Contains(expectedErrorMessage));
        };
    }
}