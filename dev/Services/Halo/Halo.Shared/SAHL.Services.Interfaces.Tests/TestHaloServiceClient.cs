using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestHaloServiceClient
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var serviceUrlConfigurationProvider = Substitute.For<IServiceUrlConfigurationProvider>();
            var jsonActivator                   = Substitute.For<IJsonActivator>();
            var webHttpClient                   = Substitute.For<IWebHttpClient>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloServiceClient = new HaloServiceClient(serviceUrlConfigurationProvider, jsonActivator, webHttpClient);
            //---------------Test Result -----------------------
            Assert.IsNotNull(haloServiceClient);
        }

        [Test]
        public void PerformCommand_ShouldCompleteSuccessfully()
        {
            //---------------Set up test pack-------------------
            var haloServiceClient = this.CreateHaloServiceClient();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => haloServiceClient.PerformQuery(new GetAllApplicationsQuery()));
            //---------------Test Result -----------------------
        }

        private IHaloServiceClient CreateHaloServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider = null,
                                                           IJsonActivator jsonActivator = null, IWebHttpClient webHttpClient = null)
        {
            var configurationProvider = serviceUrlConfigurationProvider ?? Substitute.For<IServiceUrlConfigurationProvider>();
            var activator             = jsonActivator ?? new JsonActivator();
            var httpClient            = webHttpClient ?? new FakeWebHttpClient();

            var haloServiceClient = new HaloServiceClient(configurationProvider, activator, httpClient);
            return haloServiceClient;
        }
    }
}
