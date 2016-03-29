using NUnit.Framework;

using StructureMap;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Config.Services.Halo.Client.Tests
{
    [TestFixture]
    public class TestIocContainer
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
        public void Constructor_ShouldRegisterWebHttpClient()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = ObjectFactory.GetInstance<IWebHttpClient>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
        }

        [Test]
        public void Constructor_ShouldRegisterServiceUrlConfigurationProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = ObjectFactory.GetNamedInstance<IServiceUrlConfigurationProvider>("HaloServiceUrlConfiguration");
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
        }

        [Test]
        public void Constructor_ShouldRegisterHaloServiceClient()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = ObjectFactory.GetInstance<IHaloServiceClient>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
        }
    }
}
