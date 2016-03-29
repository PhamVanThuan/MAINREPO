using NUnit.Framework;

using SAHL.Core.Services;

namespace SAHL.Core.Tests.Services
{
    [TestFixture]
    public class TestServiceUrlConfigurationProvider
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurationProvider = new ServiceUrlConfigurationProvider("Test");
            //---------------Test Result -----------------------
            Assert.IsNotNull(configurationProvider);
        }

        [Test]
        public void ServiceHostName_GivenValidConfiguration_ShouldreturnTestServiceHostName()
        {
            //---------------Set up test pack-------------------
            var configurationProvider = new ServiceUrlConfigurationProvider("TestService");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var serviceHostName = configurationProvider.ServiceHostName;
            //---------------Test Result -----------------------
            Assert.AreEqual("TestHost", serviceHostName);
        }

        [Test]
        public void ServiceName_GivenValidConfiguration_ShouldreturnTestServiceName()
        {
            //---------------Set up test pack-------------------
            var configurationProvider = new ServiceUrlConfigurationProvider("TestService");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var serviceName = configurationProvider.ServiceName;
            //---------------Test Result -----------------------
            Assert.AreEqual("TestService", serviceName);
        }
    }
}