using NUnit.Framework;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.BusinessModel.Tests
{
    [TestFixture]
    public class TestBusinessKey
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var businessKey = new BusinessKey(1, GenericKeyType.Account);
            //---------------Test Result -----------------------
            Assert.IsNotNull(businessKey);
        }

        [Test]
        public void Constructor_ShouldsetProperties()
        {
            //---------------Set up test pack-------------------
            const int key                       = 1;
            const GenericKeyType genericKeyType = GenericKeyType.Account;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var businessKey = new BusinessKey(key, genericKeyType);
            //---------------Test Result -----------------------
            Assert.AreEqual(key, businessKey.Key);
            Assert.AreEqual(genericKeyType, businessKey.KeyType);
        }
    }
}
