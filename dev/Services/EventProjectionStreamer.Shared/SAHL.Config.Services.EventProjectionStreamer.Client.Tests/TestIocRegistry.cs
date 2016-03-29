using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using StructureMap;

using SAHL.Core.Services;
using SAHL.Services.Interfaces.EventProjectionStreamer;

namespace SAHL.Config.Services.EventProjectionStreamer.Client.Tests
{
    [TestFixture]
    public class TestIocRegistry
    {
        [TestFixtureTearDown]
        public void Teardown()
        {
            ObjectFactory.Container.Dispose();
        }

        [Test]
        public void Initialize_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(this.ConfigureIoc);
            //---------------Test Result -----------------------
        }

        [Test]
        public void GetNamedInstance_GivenServiceUrlConfigurationProviderName_ShouldReturnConfiguredProvider()
        {
            //---------------Set up test pack-------------------
            this.ConfigureIoc();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurationProvider = ObjectFactory.GetNamedInstance<IServiceUrlConfigurationProvider>("EventProjectionStreamerUrlConfiguration");
            //---------------Test Result -----------------------
            Assert.IsNotNull(configurationProvider);
            Assert.AreEqual("TestHost", configurationProvider.ServiceHostName);
            Assert.AreEqual("TestService", configurationProvider.ServiceName);
        }

        [Test]
        public void GetInstance_ShouldReturnEventProjectionStreamerClient()
        {
            //---------------Set up test pack-------------------
            this.ConfigureIoc();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = ObjectFactory.GetInstance<IEventProjectionStreamerServiceClient>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }
        private void ConfigureIoc()
        {
            ObjectFactory.Initialize(expression => expression.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                    scanner.LookForRegistries();
                }));
        }
    }
}
