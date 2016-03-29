using StructureMap;
using NUnit.Framework;
using SAHL.Core.Services;

namespace SAHL.Config.Services.Tests
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
        public void Registry_ShouldRegisterHostedService()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var instance = ObjectFactory.GetInstance<IHostedService>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(instance);
        }
    }
}

