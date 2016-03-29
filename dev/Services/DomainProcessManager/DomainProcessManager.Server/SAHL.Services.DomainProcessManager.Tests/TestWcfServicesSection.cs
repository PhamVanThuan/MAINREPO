using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Services.DomainProcessManager.Configuration;
using System.Configuration;

namespace SAHL.Services.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestWcfServicesSection
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wcfServicesSection = new WcfServicesSection();
            //---------------Test Result -----------------------
            Assert.IsNotNull(wcfServicesSection);
        }

        [Test]
        public void Load_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            WcfServicesSection wcfServicesSection = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => wcfServicesSection = (WcfServicesSection)ConfigurationManager.GetSection("WcfServices"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(wcfServicesSection);
        }

        [Test]
        public void Services_ShouldContainAllServices()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wcfServicesSection = (WcfServicesSection) ConfigurationManager.GetSection("WcfServices");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, wcfServicesSection.Services.Count);
        }

        [Test]
        public void Service_GivenServiceName_ShouldreturnWcfService()
        {
            //---------------Set up test pack-------------------
            var wcfServicesSection = (WcfServicesSection)ConfigurationManager.GetSection("WcfServices");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = wcfServicesSection.Services["Domain Process Manager Service"];
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }

        [Test]
        public void Properties_GivenValidWcfService_ShouldHaveAllPropertiesSetup()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wcfServicesSection = (WcfServicesSection)ConfigurationManager.GetSection("WcfServices");
            //---------------Test Result -----------------------
            var serviceElement = wcfServicesSection.Services[0];

            Assert.IsNotNull(serviceElement);
            Assert.IsNotNullOrEmpty(serviceElement.Name);
            Assert.AreEqual("Domain Process Manager Service", serviceElement.Name);

            Assert.IsNotNullOrEmpty(serviceElement.Interface);
            Assert.AreEqual("SAHL.Services.DomainProcessManager.WcfService.IDomainProcessManagerService, SAHL.Services.DomainProcessManager", serviceElement.Interface);

            Assert.IsNotNullOrEmpty(serviceElement.Address);
            Assert.AreEqual("http://localhost:9001/SAHL/Services/DomainProcessManager", serviceElement.Address);
        }
    }
}
