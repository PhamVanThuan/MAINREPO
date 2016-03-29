using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Identity.Tests
{
    [TestFixture]
    public class TestUserManager
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            SAHL.Core.Testing.Ioc.TestingIoc.Initialise();
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var userRepository = Substitute.For<IUserRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userManager = new UserManager(userRepository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(userManager);
        }

        [TestCase("userRepository")]
        public void Construct_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<UserManager>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void GetUserDetails_GivenEmptyAdUserName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var userManager = this.CreateUserManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => userManager.GetUserDetails(string.Empty));
            //---------------Test Result -----------------------
            StringAssert.Contains("adUserName", exception.ParamName);
        }

        [Test]
        public void GetUserDetails_GivenUnknownUser_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var userManager = this.CreateUserManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userDetails = userManager.GetUserDetails("UnknownTestUser");
            //---------------Test Result -----------------------
            Assert.IsNull(userDetails);
        }

        [Test]
        public void GetUserDetails_GivenExistingUser_ShouldReturnUserDetails()
        {
            //---------------Set up test pack-------------------
            var userManager = this.CreateUserManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userDetails = userManager.GetUserDetails(@"SAHL\BCUser");
            //---------------Test Result -----------------------
            Assert.IsNotNull(userDetails);
        }

        [Test]
        public void GetUserDetails_GivenExistingUser_ShouldReturnExpectedUserDetails()
        {
            //---------------Set up test pack-------------------
            var userManager = this.CreateUserManager();
            var adUserName = @"SAHL\BCUser";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userDetails = userManager.GetUserDetails(adUserName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(userDetails);
            Assert.AreEqual(adUserName, userDetails.FullADUsername);
            Assert.AreEqual("BranchConsultantUser", userDetails.DisplayName);
            Assert.Greater(userDetails.UserRoles.Count(), 0);
        }

        private IUserManager CreateUserManager(IUserRepository userRepository = null)
        {
            FakeDbFactory dbFactory = new FakeDbFactory();

            List<UserRole> roles = new List<UserRole>();
            roles.Add(new UserRole { OrganisationArea = "A", RoleName = "b" });
            dbFactory.FakedDb.DbReadOnlyContext.Select(Arg.Any<GetUserRoleStatement>()).Returns(roles);
            var userManager = new UserManager(userRepository ?? new UserRepository(dbFactory));
            return userManager;
        }
    }
}