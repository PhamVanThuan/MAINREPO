using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.DomainProcess;
using SAHL.Core.IoC;
using SAHL.Core.Messaging;
using SAHL.Services.DomainProcessManager.Data;
using SAHL.Services.DomainProcessManager.Services;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.DomainProcessManager.Server.Tests
{
    [TestFixture]
    public class TestIocRegistry
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression => expression.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                }));
        }

        [Test]
        public void Constructor_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var iocRegistry = new IocRegistry();
            //---------------Test Result -----------------------
            Assert.IsNotNull(iocRegistry);
        }

        [TestCase(typeof(DomainProcessEventHandlerService))]
        [TestCase(typeof(DomainProcessCoordinatorService))]
        public void ObjectFactory_ShouldRegisterStartableInIoc(Type serviceType)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var startableServices = ObjectFactory.GetAllInstances<IStartable>();
            //---------------Test Result -----------------------
            var startableService = startableServices.FirstOrDefault(service => service.GetType() == serviceType);
            Assert.IsNotNull(startableService);
        }

        [TestCase(typeof(DomainProcessEventHandlerService))]
        [TestCase(typeof(DomainProcessCoordinatorService))]
        public void ObjectFactory_ShouldRegisterStoppableInIoc(Type serviceType)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var stoppableServices = ObjectFactory.GetAllInstances<IStoppable>();
            //---------------Test Result -----------------------
            var stoppableService = stoppableServices.FirstOrDefault(service => service.GetType() == serviceType);
            Assert.IsNotNull(stoppableService);
        }

        [Test]
        public void ObjectFactory_ShouldRegisterMessagebus()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageBusAdvanced = ObjectFactory.GetInstance<IMessageBusAdvanced>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(messageBusAdvanced);
        }

        [Test]
        public void ObjectFactory_ShouldRegisterDomainProcessRepository()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = ObjectFactory.GetInstance<IDomainProcessRepository>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
        }

        [Test]
        public void ObjectFactory_ShouldRegisterDomainProcessEventHandlerService()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = ObjectFactory.GetInstance<IDomainProcessEventHandlerService>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }

        [Test]
        public void ObjectFactory_ShouldRegisterDomainProcessCoordinatorService()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = ObjectFactory.GetInstance<IDomainProcessCoordinatorService>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }

        [Test]
        public void ObjectFactory_ShouldRegisterUIStatementsProviderForDomainProcessManager()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var providers = ObjectFactory.GetAllInstances<IUIStatementsProvider>();
            //---------------Test Result -----------------------
            Assert.IsTrue(providers.Any(provider => provider.GetType().Name == typeof(DomainProcessUIStatementProvider).Name));
        }
    }
}