using NUnit.Framework;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;

namespace SAHL.Config.Services.WorkflowAssignmentDomain.Client.Tests
{
    [TestFixture]
    public class IocRegistryTests
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            this.iocContainer = new ServiceBootstrapper().Initialise();
        }

        [Test]
        public void Constructor_ShouldRegisterServiceUrlConfigurationProvider()
        {
            var instance = this.iocContainer.GetInstance<IServiceUrlConfigurationProvider>("WorkflowAssignmentDomainServiceUrlConfiguration");

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Constructor_ShouldRegisterServiceClient()
        {
            var instance = this.iocContainer.GetInstance<IWorkflowAssignmentDomainServiceClient>();

            Assert.That(instance, Is.Not.Null);
        }
    }
}
