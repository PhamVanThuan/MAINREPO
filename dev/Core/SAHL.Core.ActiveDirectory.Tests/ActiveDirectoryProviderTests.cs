using NSubstitute;
using NUnit.Framework;
using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.ActiveDirectory.Query;
using System;
using System.Linq;

namespace SAHL.Core.ActiveDirectory.Tests
{
    [TestFixture]
    public class ActiveDirectoryProviderTests
    {
        [Test]
        public void GetMemberOfInfo_GivenNullUsername_ShouldReturnEmptyEnumerable()
        {
            var provider = new ActiveDirectoryProvider(new ActiveDirectoryQueryFactory(), new ActiveDirectoryProviderCache(), new DefaultCredentials());

            var roles = provider.GetMemberOfInfo(null);

            Assert.IsEmpty(roles);
        }

        [Test]
        public void GetMemberOfInfo_GivenWhitespaceUsername_ShouldReturnEmptyEnumerable()
        {
            var provider = new ActiveDirectoryProvider(new ActiveDirectoryQueryFactory(), new ActiveDirectoryProviderCache(), new DefaultCredentials());

            var roles = provider.GetMemberOfInfo("     ");

            Assert.IsEmpty(roles);
        }

        [Test]
        public void GetMemberOfInfo_GivenInvalidUsername_ShouldReturnEmptyEnumerable()
        {
            var provider = new ActiveDirectoryProvider(new ActiveDirectoryQueryFactory(), new ActiveDirectoryProviderCache(), new DefaultCredentials());

            var roles = provider.GetMemberOfInfo(Guid.NewGuid().ToString());

            Assert.IsEmpty(roles);
        }

        [Test]
        public void GetMemberOfInfo_GivenNullResultsFromQuery_ShouldReturnZeroRoles()
        {
            const string username = "fakeUser";

            var credentials = Substitute.For<ICredentials>();

            var query = Substitute.For<IActiveDirectoryQuery>();
            query.FindAll(null).Returns(a => null); //can't return anything other than null as SearchResultCollection does not have a public constructor for mocking

            var factory = Substitute.For<IActiveDirectoryQueryFactory>();
            factory.Create(credentials, "(&(SAMAccountName={0}))").Returns(a => query);

            var provider = new ActiveDirectoryProvider(factory, new ActiveDirectoryProviderCache(), credentials);

            var roles = provider.GetMemberOfInfo(username);

            Assert.AreEqual(0, roles.Count());
        }
    }
}