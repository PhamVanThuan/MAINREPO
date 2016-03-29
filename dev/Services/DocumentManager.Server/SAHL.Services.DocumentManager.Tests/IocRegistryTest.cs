using NUnit.Framework;
using SAHL.Config.Services.DocumentManager.Server;
using StructureMap;
using System;
using System.IO.Abstractions;
using System.Linq;

namespace SAHL.Services.UserProfile.Tests
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
        public void ObjectFactory_ShouldContain_IFileSystem()
        {
            var fileSystem = ObjectFactory.GetInstance<IFileSystem>();
            Assert.IsNotNull(fileSystem);
        }
    }
}