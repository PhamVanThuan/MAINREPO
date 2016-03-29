using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Query;
using StructureMap;

namespace SAHL.Config.Services.Query.Client.Tests
{
    [TestFixture]
    public class IocRegistryTests
    {
        [Test]
        public void Constructor_ShouldRegisterServiceUrlConfigurationProvider()
        {
            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Initialise();

            var instance = ObjectFactory.GetNamedInstance<IServiceUrlConfigurationProvider>("QueryServiceUrlConfiguration");

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Constructor_ShouldRegisterServiceClient()
        {
            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Initialise();

            var instance = ObjectFactory.GetInstance<IQueryServiceClient>();

            Assert.That(instance, Is.Not.Null);
        }
    }
}
