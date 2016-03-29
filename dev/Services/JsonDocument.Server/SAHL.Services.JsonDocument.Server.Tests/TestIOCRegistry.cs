using NUnit.Framework;
using SAHL.Config.Services.JsonDocument.Server;
using StructureMap;
using System;
using System.Linq;

namespace SAHL.Services.JsonDocument.Server.Tests
{
    [TestFixture]
    public class TestIOCRegistry
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
    }
}