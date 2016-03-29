using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap;
using NUnit.Framework;

using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;

namespace SAHL.Config.DomainManager.DomainProcesses.Tests
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

        [Test]
        public void ObjectFactory_ShouldRegisterDomainProcessInIoc()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var domainProcesses = ObjectFactory.GetAllInstances<IDomainProcess<FakeDomainModel>>();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, domainProcesses.Count);
        }

        [Test]
        public void ObjectFactory_ShouldRegisterNewPurchaseApplicationDomainProcess()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var domainProcess = ObjectFactory.GetInstance<NewPurchaseApplicationDomainProcess>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainProcess);
        }
    }
}