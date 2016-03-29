using NSubstitute;
using NUnit.Framework;
using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.ActiveDirectory.Query.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Tests
{
    [TestFixture]
    public class MemberOfInfoTests
    {
        [Test]
        public void GetMemberOfInfo_GivenValidUsername_ShouldReturnRoleForGivenUser()
        {
            const string username = "fakeUser";
            var memberOfInfoList = new List<MemberOfInfo>
            {
                new MemberOfInfo("CN=Group1"),
            };

            var provider = Substitute.For<IActiveDirectoryProvider>();
            provider.GetMemberOfInfo(username).Returns(a => memberOfInfoList);

            var roles = provider.GetMemberOfInfo(username);

            Assert.AreEqual(1, roles.Count());
            Assert.AreEqual("Group1", roles.First().CommonName);
        }

        [Test]
        public void GetMemberOfInfo_GivenValidUsername_ShouldReturnMultipleRolesForGivenUser()
        {
            const string username = "fakeUser";
            var memberOfInfoList = new List<MemberOfInfo>
            {
                new MemberOfInfo("CN=Group1"),
                new MemberOfInfo("CN=Group2"),
                new MemberOfInfo("CN=Group3"),
                new MemberOfInfo("CN=Group4"),
            };

            var provider = Substitute.For<IActiveDirectoryProvider>();
            provider.GetMemberOfInfo(username).Returns(a => memberOfInfoList);

            var roles = provider.GetMemberOfInfo(username).ToList();

            Assert.AreEqual(4, roles.Count());
            Assert.AreEqual("Group1", roles[0].CommonName);
            Assert.AreEqual("Group2", roles[1].CommonName);
            Assert.AreEqual("Group3", roles[2].CommonName);
            Assert.AreEqual("Group4", roles[3].CommonName);
        }

        [Test]
        public void GetMemberOfInfo_GivenValidUsernameAndFullResultString_ShouldReturnRoleForGivenUser()
        {
            const string username = "fakeUser";
            var memberOfInfoList = new List<MemberOfInfo>
            {
                new MemberOfInfo("CN=IT Developers,OU=Groups &  Service acc's,OU=IT,OU=Head Office,DC=SAHL,DC=com"),
            };

            var provider = Substitute.For<IActiveDirectoryProvider>();
            provider.GetMemberOfInfo(username).Returns(a => memberOfInfoList);

            var roles = provider.GetMemberOfInfo(username);

            Assert.AreEqual(1, roles.Count());
            Assert.AreEqual("IT Developers", roles.First().CommonName);
        }

        [Test]
        public void GetMemberOfInfo_GivenCustomTransformer_ShouldReturnRoleForGivenUserInTransformedType()
        {
            const string username = "fakeUser";
            var memberOfInfoList = new List<string>
            {
                "Group1"
            };

            Func<string, SecurityIdentifier, string> resultTransformer = (a, b) => a; //i.e. perform no transform on result string, return raw

            var provider = Substitute.For<IActiveDirectoryProvider>();
            provider.GetMemberOfInfo(username, resultTransformer).Returns(a => memberOfInfoList);

            var roles = provider.GetMemberOfInfo(username, resultTransformer);

            Assert.AreEqual(1, roles.Count());
            Assert.AreEqual("Group1", roles.First());
        }

        [Test]
        public void GetMemberOfInfo_GivenValidUsernameAndFullResultString_ShouldPopulateSecurityIdentifier()
        {
            const string username = "fakeUser";
            var memberOfInfoList = new List<MemberOfInfo>
            {
                new MemberOfInfo("CN=IT Developers,OU=Groups &  Service acc's,OU=IT,OU=Head Office,DC=SAHL,DC=com"),
            };

            var provider = Substitute.For<IActiveDirectoryProvider>();
            provider.GetMemberOfInfo(username).Returns(a => memberOfInfoList);

            var roles = provider.GetMemberOfInfo(username);

            Assert.AreEqual(1, roles.Count());
            Assert.AreEqual("IT Developers", roles.First().CommonName);
        }
    }
}