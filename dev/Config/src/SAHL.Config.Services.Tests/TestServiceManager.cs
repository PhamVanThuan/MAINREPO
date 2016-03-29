using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.IoC;
using SAHL.Config.Services.Core;

namespace SAHL.Config.Services.Tests
{
    [TestFixture]
    public class TestServiceManager
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var serviceManager = new ServiceManager(iocContainer);
            //---------------Test Result -----------------------
            Assert.IsNotNull(serviceManager);
        }

        [Test]
        public void StartService_GivenNoStartableOrStartableService_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var serviceManager = this.CreateServiceManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(serviceManager.StartService);
            //---------------Test Result -----------------------
        }

        [Test]
        public void StopService_GivenNoStoppableOrStoppableService_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var serviceManager = this.CreateServiceManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(serviceManager.StopService);
            //---------------Test Result -----------------------
        }

        private IServiceManager CreateServiceManager(IIocContainer iocContainer = null)
        {
            IIocContainer container = iocContainer;
            if (container == null)
            {
                container = Substitute.For<IIocContainer>();

                container.GetAllInstances<IStartable>().Returns(info => new List<IStartable>());
                container.GetAllInstances<IStoppable>().Returns(info => new List<IStoppable>());

                container.GetInstance<IStartableService>().Returns(info => null);
                container.GetInstance<IStoppableService>().Returns(info => null);
            }

            var serviceManager = new ServiceManager(container);
            return serviceManager;
        }
    }
}
