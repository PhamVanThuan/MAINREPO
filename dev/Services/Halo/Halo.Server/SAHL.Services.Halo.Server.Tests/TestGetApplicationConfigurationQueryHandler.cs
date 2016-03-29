using System;
using System.Linq;

using NSubstitute;
using NUnit.Framework;

using SAHL.UI.Halo.Shared;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestGetApplicationConfigurationQueryHandler : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryHandler = new GetApplicationConfigurationQueryHandler(repository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryHandler);
        }

        [Test]
        public void Constructor_GivenNullTileConfigurationRepository_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetApplicationConfigurationQueryHandler(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("tileConfigurationRepository", exception.ParamName);
        }

        [Test]
        public void HandleQuery_GivenRepositoryThrowsException_ShouldReturnExceptionDetailsInSystemCollection()
        {
            //---------------Set up test pack-------------------
            const string testExceptionMessage = "Test exception message";
            var repository                    = Substitute.For<ITileConfigurationRepository>();

            repository.FindApplicationConfiguration(Arg.Any<string>()).Returns(info =>
                {
                    throw new Exception(testExceptionMessage);
                });

            var systemMessageCollection = SystemMessageCollection.Empty();
            var queryHandler            = new GetApplicationConfigurationQueryHandler(repository);
            var query                   = new GetApplicationConfigurationQuery("application");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => { systemMessageCollection = queryHandler.HandleQuery(query); });
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsTrue(systemMessageCollection.HasErrors);
            Assert.AreEqual(1, systemMessageCollection.AllMessages.Count());
            StringAssert.Contains(testExceptionMessage, systemMessageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenValidConfiguration_ShouldReturnApplicationConfiguration()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "My Halo";
            var repository               = new TileConfigurationRepository(this.iocContainer);
            var queryHandler             = this.CreateQueryHandler(repository);
            var query                    = new GetApplicationConfigurationQuery(applicationName);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            var expectedApplicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            Assert.Greater(query.Result.Results.Count(), 0);
            Assert.AreEqual(expectedApplicationConfiguration.Name, query.Result.Results.First().HaloApplicationModel.Name);
        }

        [Test]
        public void HandleQuery_GivenValidConfiguration_ShouldReturnApplicationConfigurationWithModules()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "My Halo";
            var repository               = new TileConfigurationRepository(this.iocContainer);
            var queryHandler             = this.CreateQueryHandler(repository);
            var query                    = new GetApplicationConfigurationQuery(applicationName);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            var applicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            Assert.IsNotNull(applicationConfiguration);

            var moduleConfigurations = repository.FindApplicationModuleConfigurations(applicationConfiguration);
            Assert.IsTrue(moduleConfigurations.Any());

            var queryResult = query.Result.Results.FirstOrDefault();
            Assert.IsNotNull(queryResult);

            var returnedApplicationConfiguration = query.Result.Results.First().HaloApplicationModel;
            Assert.IsNotNull(returnedApplicationConfiguration);

            Assert.IsTrue(returnedApplicationConfiguration.Modules.Any());
            Assert.AreEqual(moduleConfigurations.Count(), returnedApplicationConfiguration.Modules.Count());
            Assert.IsNotNull(returnedApplicationConfiguration.Modules.FirstOrDefault(model => model.Name == moduleConfigurations.First().Name));
        }

        private GetApplicationConfigurationQueryHandler CreateQueryHandler(ITileConfigurationRepository tileConfigurationRepository = null)
        {
            var configurationRepository = tileConfigurationRepository ?? new TileConfigurationRepository(this.iocContainer);
            var queryHandler            = new GetApplicationConfigurationQueryHandler(configurationRepository);
            return queryHandler;
        }
    }
}
