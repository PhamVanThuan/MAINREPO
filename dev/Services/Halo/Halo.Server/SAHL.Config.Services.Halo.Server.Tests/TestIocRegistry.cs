using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.IoC;

namespace SAHL.Config.Services.Halo.Server.Tests
{
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
        public void Registry_ShouldRegisterIAutoMapperProfiles()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allInstances = ObjectFactory.GetAllInstances<IAutoMapperProfile>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(allInstances);
            Assert.AreEqual(10, allInstances.Count);
        }

        [Test]
        public void Registry_ShouldRegisterIStartables()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allInstances = ObjectFactory.GetAllInstances<IStartable>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(allInstances);
            Assert.AreEqual(1, allInstances.Count);
        }
    }
}
