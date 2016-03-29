using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SAHL.Config.Services.Communications.Server;
using SAHL.Core.IoC;
using SAHL.Services.Communications.Managers.Email;
using SAHL.Services.Communications.Managers.LiveReplies;
using StructureMap;

namespace SAHL.Services.Communications.Tests
{
    [TestFixture]
    public class TestIocRegistry
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression => expression.Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL") && !y.FullName.Contains("SAHL.Core.Testing"));
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.LookForRegistries();
            }));
        }

        [Test]
        public void IOCRegistry_CanBeCreatedWithoutException()
        {
            //------Execute ------
            var iocRegistry = new IocRegistry();
            //------Result -------
            Assert.IsNotNull(iocRegistry);
        }

        [Test]
        public void IOCRegistry_Contains_LiveRepliesManager_As_Startable()
        {
            IList<IStartable> startables = ObjectFactory.GetAllInstances<IStartable>();
            Assert.That(startables.First(x => x.GetType().Name.Contains(typeof(LiveRepliesManager).Name)) != null);
        }

        [Test]
        public void IOCRegistry_Contains_LiveRepliesManager_As_Stoppable()
        {
            IList<IStoppable> startables = ObjectFactory.GetAllInstances<IStoppable>();
            Assert.That(startables.First(x => x.GetType().Name.Contains(typeof(LiveRepliesManager).Name)) != null);
        }

        [Test]
        public void IOCRegistry_Contains_ICommunicationSettings_As_CommunicationSettings()
        {
            var instance = ObjectFactory.GetInstance<ICommunicationSettings>();
            Assert.That(instance.GetType().Name.Contains(typeof(CommunicationSettings).Name));
        }
    }
}
