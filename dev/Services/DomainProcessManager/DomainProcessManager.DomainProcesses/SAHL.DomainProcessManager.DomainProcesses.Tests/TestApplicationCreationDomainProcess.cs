using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Event;
using SAHL.DomainProcessManager.DomainProcesses.Model;

namespace SAHL.DomainProcessManager.DomainProcesses.Tests
{
    [TestFixture]
    public class TestApplicationCreationDomainProcess
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var domainProcess = new ApplicationCreationDomainProcess();
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainProcess);
        }

        [Test]
        public void Start_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var domainProcess = this.CreateDomainProcess();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => domainProcess.Start(new ApplicationCreationModel()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Handle_GivenBasicApplicationCreatedEvent_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var domainProcess = this.CreateDomainProcess();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => domainProcess.Handle(new BasicApplicationCreatedEvent(Guid.NewGuid(), Guid.NewGuid())));
            //---------------Test Result -----------------------
        }

        private ApplicationCreationDomainProcess CreateDomainProcess()
        {
            var domainProcess = new ApplicationCreationDomainProcess();
            return domainProcess;
        }
    }
}
