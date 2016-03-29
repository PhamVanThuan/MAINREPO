using NUnit.Framework;

namespace SAHL.Core.ActiveDirectory.Tests
{
    [TestFixture]
    public class UsernameHelpersTests
    {
        [Test]
        public void RemoveDomainPrefixIfAny_GivenNullUsername_ShouldReturnSame()
        {
            const string username = null;

            var newUsername = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            Assert.IsNull(newUsername);
        }

        [Test]
        public void RemoveDomainPrefixIfAny_GivenEmptyUsername_ShouldReturnSame()
        {
            const string username = "";

            var newUsername = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            Assert.AreEqual(username, newUsername);
        }

        [Test]
        public void RemoveDomainPrefixIfAny_GivenWhitespaceUsername_ShouldReturnSame()
        {
            const string username = "     ";

            var newUsername = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            Assert.AreEqual(username, newUsername);
        }

        [Test]
        public void RemoveDomainPrefixIfAny_GivenNonPrefixedUsername_ShouldReturnSame()
        {
            const string username = "bob";

            var newUsername = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            Assert.AreEqual(username, newUsername);
        }

        [Test]
        public void RemoveDomainPrefixIfAny_GivenPrefixedUsername_ShouldReturnNonPrefixedUsername()
        {
            const string username = @"sahl\bob";

            var newUsername = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            Assert.AreEqual("bob", newUsername);
        }

        [Test]
        public void RemoveDomainPrefixIfAny_GivenInvalidUsername_ShouldReturnEmptyString()
        {
            const string username = @"\";

            var newUsername = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            Assert.AreEqual("", newUsername);
        }

        [Test]
        public void RemoveDomainPrefixIfAny_GivenMultipleSlashes_ShouldReturnStringWithFirstTokenAndSlashRemoved()
        {
            const string username = @"some\more\slashes";

            var newUsername = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            Assert.AreEqual(@"more\slashes", newUsername);
        }
    }
}