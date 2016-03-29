using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Identity.Specs.UserManagerSpecs
{
    public class when_retrieving_existing_user_details : WithFakes
    {
        private static string _adUserName;
        private static IUserRepository _userRepository;
        private static UserManager _userManager;
        private static IUserDetails _userDetailsResult;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            SAHL.Core.Testing.Ioc.TestingIoc.Initialise();

            _adUserName = @"SAHL\BCUser";

            dbFactory = new FakeDbFactory();
            _userRepository = new UserRepository(dbFactory);

            List<UserRole> roles = new List<UserRole>();
            roles.Add(new UserRole() { OrganisationArea = "A", RoleName = "b" });

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<UserRole>(Param.IsAny<GetUserRoleStatement>())).Return(roles);

            _userManager = new UserManager(_userRepository);
        };

        private Because of = () =>
        {
            _userDetailsResult = _userManager.GetUserDetails(_adUserName);
        };

        private It should_return_user_details = () =>
        {
            _userDetailsResult.ShouldNotBeNull();
        };

        private It should_set_relevant_properties = () =>
        {
            _userDetailsResult.FullADUsername.ShouldEqual(_adUserName);
            _userDetailsResult.DisplayName.ShouldEqual("BranchConsultantUser");
            _userDetailsResult.EmailAddress.ShouldBeEmpty();
            _userDetailsResult.UserPhoto.ShouldBeNull();
        };

        private It should_load_user_roles = () =>
        {
            _userDetailsResult.UserRoles.Count().ShouldBeGreaterThan(0);
        };
    }
}