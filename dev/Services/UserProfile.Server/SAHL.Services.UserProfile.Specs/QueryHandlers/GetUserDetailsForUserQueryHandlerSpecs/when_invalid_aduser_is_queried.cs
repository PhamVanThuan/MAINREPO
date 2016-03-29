using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.UserProfile.Queries;
using SAHL.Services.UserProfile.QueryHandlers;
using System;
using System.Linq;

namespace SAHL.Services.UserProfile.Specs.QueryHandlers.GetUserDetailsForUserQueryHandlerSpecs
{
    [Subject("SAHL.Services.UserProfile.QueryHandlers.GetUserDetailsForUserQueryHandler.Handle")]
    public class when_invalid_aduser_is_queried : WithFakes
    {
        private static IUserManager userManager;
        private static GetUserDetailsForUserQuery query;
        private static GetUserDetailsForUserQueryHandler queryHandler;
        private static string _username;
        private static Exception exception;

        private Establish context = () =>
        {
            _username = "invalid";
            userManager = An<IUserManager>();
            IUserDetails userDetails = null;
            userManager.WhenToldTo(x => x.GetUserDetails(_username)).Return(userDetails);
            query = new GetUserDetailsForUserQuery(_username);
            queryHandler = new GetUserDetailsForUserQueryHandler(userManager);
        };

        private Because of = () =>
        {
            exception = Machine.Specifications.Catch.Exception(() => queryHandler.HandleQuery(query));
        };

        private It should_not_set_result = () =>
        {
            query.Result.ShouldBeNull();
        };

        private It should_get_user_details_from_user_manager = () =>
        {
            userManager.WasToldTo(x => x.GetUserDetails(_username));
        };

        private It should_throw_exception_error = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}