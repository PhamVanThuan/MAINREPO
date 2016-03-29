using NSubstitute;
using NUnit.Framework;
using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Query;

namespace SAHL.Core.ActiveDirectory.Tests
{
    [TestFixture]
    public class ActiveDirectoryQueryTests
    {
        [Test]
        public void Constructor_GivenFilter_FilterIsCorrectlySetOnDirectorySearcher()
        {
            var credentials = Substitute.For<ICredentials>();

            var factory = new ActiveDirectoryQueryFactory();
            const string filter = "MyTestFilter";

            var query = (ActiveDirectoryQuery)factory.Create(credentials, filter);

            Assert.AreEqual(filter, query.Filter);
        }
    }
}