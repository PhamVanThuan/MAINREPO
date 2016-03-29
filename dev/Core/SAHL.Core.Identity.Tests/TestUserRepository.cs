using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Identity.Tests
{
    [TestFixture]
    public class TestUserRepository
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
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDbFactory dbFactory = new FakeDbFactory();
            var userRepository = new UserRepository(dbFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(userRepository);
        }

        [Test]
        public void ADFindUser_GivenUserThatDoesNotExistInAD_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IDbFactory dbFactory = new FakeDbFactory();
            var userRepository = new UserRepository(dbFactory);
            var userDetailsInput = new UserDetails
                {
                    FullADUsername = "Bob",
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userDetailsResult = userRepository.ADFindUser(userDetailsInput);
            //---------------Test Result -----------------------
            Assert.IsNull(userDetailsResult);
        }

        [Test]
        public void ADFindUser_GivenUserThatExistInAD_ShouldReturnUserDetails()
        {
            //---------------Set up test pack-------------------
            IDbFactory dbFactory = new FakeDbFactory();
            var userRepository = new UserRepository(dbFactory);
            var userDetailsInput = new UserDetails
                {
                    FullADUsername = @"SAHL\BCUser",
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userDetailsResult = userRepository.ADFindUser(userDetailsInput);
            //---------------Test Result -----------------------
            Assert.IsNotNull(userDetailsResult);
            Assert.AreEqual(userDetailsInput.UserName, userDetailsResult.UserName);
            Assert.AreEqual("BranchConsultantUser", userDetailsResult.DisplayName);
            Assert.AreEqual(string.Empty, userDetailsResult.EmailAddress);
            Assert.IsNull(userDetailsResult.UserPhoto);
        }

        [Test]
        public void FindUserRoles_GivenUserThatDoesNotExist_ShouldReturnZeroUserRoles()
        {
            //---------------Set up test pack-------------------
            IDbFactory dbFactory = new FakeDbFactory();
            var userRepository = new UserRepository(dbFactory);
            var userDetailsInput = new UserDetails
                {
                    UserName = "bob",
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userDetailsResult = userRepository.FindUserRoles(userDetailsInput);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, userDetailsResult.UserRoles.Count());
        }

        [Test]
        public void FindUserRoles_GivenUserThatDoesNotExist_ShouldReturnMoreThanZeroUserRoles()
        {
            //---------------Set up test pack-------------------
            FakeDbFactory dbFactory = new FakeDbFactory();
            var userRepository = new UserRepository(dbFactory);
            var userDetailsInput = new UserDetails
                {
                    FullADUsername = @"SAHL\BCUser",
                };

            List<UserRole> roles = new List<UserRole>();
            roles.Add(new UserRole() { OrganisationArea = "A", RoleName = "b" });

            dbFactory.FakedDb.DbReadOnlyContext.Select<UserRole>(Arg.Any<GetUserRoleStatement>()).Returns(roles);

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var userDetailsResult = userRepository.FindUserRoles(userDetailsInput);
            //---------------Test Result -----------------------
            Assert.Greater(userDetailsResult.UserRoles.Count(), 0);
        }
    }
}