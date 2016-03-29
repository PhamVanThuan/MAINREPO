using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowTask;
using StructureMap;

namespace SAHL.Config.Services.WorkflowTask.Client.Tests
{
    [TestFixture]
    public class IocRegistryTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            new ServiceBootstrapper().Initialise();
        }

        [Test]
        public void Constructor_ShouldRegisterWebHttpClient()
        {
            var instance = ObjectFactory.GetInstance<IWebHttpClient>();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Constructor_ShouldRegisterServiceUrlConfigurationProvider()
        {
            var instance = ObjectFactory.GetNamedInstance<IServiceUrlConfigurationProvider>("WorkflowTaskServiceUrlConfiguration");

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Constructor_ShouldRegisterServiceClient()
        {
            var instance = ObjectFactory.GetInstance<IWorkflowTaskServiceClient>();

            Assert.That(instance, Is.Not.Null);
        }
    }
}
