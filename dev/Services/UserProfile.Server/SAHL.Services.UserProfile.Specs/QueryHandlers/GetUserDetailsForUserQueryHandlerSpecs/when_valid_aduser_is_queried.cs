using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.UserProfile.Queries;
using SAHL.Services.UserProfile.QueryHandlers;

namespace SAHL.Services.UserProfile.Specs.QueryHandlers.GetUserDetailsForUserQueryHandlerSpecs
{
    [Subject("SAHL.Services.UserProfile.QueryHandlers.GetUserDetailsForUserQueryHandler.Handle")]
    public class when_valid_aduser_is_queried : WithFakes
    {
        private static IUserManager userManager;
        private static GetUserDetailsForUserQuery query;
        private static GetUserDetailsForUserQueryHandler queryHandler;
        private static string _username;

        private Establish context = () =>
        {
            _username = "test";
            query = new GetUserDetailsForUserQuery(_username);
            userManager = An<IUserManager>();

            IUserDetails userDetails = An<IUserDetails>();

            userManager.WhenToldTo(x => x.GetUserDetails(_username)).Return(userDetails);

            queryHandler = new GetUserDetailsForUserQueryHandler(userManager);
        };

        private Because of = () =>
        {
            queryHandler.HandleQuery(query);
        };

        private It should_set_result = () =>
        {
            query.Result.ShouldNotBeNull();
        };

        private It should_get_user_details_from_user_manager = () =>
        {
            userManager.WasToldTo(x => x.GetUserDetails(_username));
        };
    }
}