using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Core.Identity.Specs.UserManagerSpecs
{
    public class when_retrieving_unknown_user_details : WithFakes
    {
        private static string _adUserName;
        private static IUserRepository _userRepository;
        private static UserManager _userManager;
        private static IUserDetails _userDetailsResult;
        private static IDbFactory dbFactory;

        private Establish context = () =>
        {
            SAHL.Core.Testing.Ioc.TestingIoc.Initialise();

            _adUserName = "UnknownTestUser";
            dbFactory = new FakeDbFactory();
            _userRepository = new UserRepository(dbFactory);
            _userManager = new UserManager(_userRepository);
        };

        private Because of = () =>
        {
            _userDetailsResult = _userManager.GetUserDetails(_adUserName);
        };

        private It should_not_return_user_details = () =>
        {
            _userDetailsResult.ShouldBeNull();
        };
    }
}