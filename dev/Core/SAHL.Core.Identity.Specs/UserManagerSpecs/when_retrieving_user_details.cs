using Machine.Fakes;
using Machine.Specifications;

namespace SAHL.Core.Identity.Specs.UserManagerSpecs
{
    public class when_retrieving_user_details : WithFakes
    {
        private static string _adUserName;
        private static IUserRepository _userRepository;
        private static UserManager _userManager;
        private static IUserDetails _userDetailsResult;
        private static IUserDetails _expectedResult;

        private Establish context = () =>
        {
            _adUserName = "Domain\\BCUser";

            _expectedResult = An<IUserDetails>();
            _expectedResult.WhenToldTo(details => details.UserName).Return(Param<string>.IsAnything);

            _userRepository = An<IUserRepository>();
            _userRepository.WhenToldTo(repository => repository.ADFindUser(Param<IUserDetails>.IsAnything)).Return(_expectedResult);

            _userManager = new UserManager(_userRepository);
        };

        private Because of = () =>
        {
            _userDetailsResult = _userManager.GetUserDetails(_adUserName);
        };

        private It should_find_ad_user_details = () =>
        {
            _userRepository.WasToldTo(x => x.ADFindUser(Param<IUserDetails>.IsAnything));
        };

        private It should_get_user_roles = () =>
        {
            _userRepository.WasToldTo(x => x.FindUserRoles(Param<IUserDetails>.IsAnything));
        };

        private It should_return_user_details = () =>
        {
            _userDetailsResult.ShouldNotBeNull();
        };
    }
}