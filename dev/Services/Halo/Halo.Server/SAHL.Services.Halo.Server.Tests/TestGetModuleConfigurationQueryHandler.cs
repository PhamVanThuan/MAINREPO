using System;
using System.Linq;

using NSubstitute;
using NUnit.Framework;

using SAHL.UI.Halo.Shared;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo.Queries;
using SAHL.Services.Halo.Server.QueryHandlers;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestGetModuleConfigurationQueryHandler : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryHandler = new GetModuleConfigurationQueryHandler(repository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryHandler);
        }

        [Test]
        public void Constructor_GivenNullTileConfigurationRepository_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetModuleConfigurationQueryHandler(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("tileConfigurationRepository", exception.ParamName);
        }

        [Test]
        public void HandleQuery_GivenRepositoryThrowsException_ShouldReturnExceptionDetailsInSystemCollection()
        {
            //---------------Set up test pack-------------------
            const string testExceptionMessage = "Test exception message";
            var repository = Substitute.For<ITileConfigurationRepository>();

            repository.FindApplicationConfiguration(Arg.Any<string>()).Returns(info =>
                {
                    throw new Exception(testExceptionMessage);
                });

            var systemMessageCollection = SystemMessageCollection.Empty();
            var queryHandler            = new GetModuleConfigurationQueryHandler(repository);
            var query                   = new GetModuleConfigurationQuery("application", "module");
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
        public void HandleQuery_GivenApplicationConfigurationNotFound_ShouldReturnErrorInMessageCollection()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileConfigurationRepository>();
            repository.FindApplicationConfiguration(Arg.Any<string>()).Returns(info => null);

            var query        = new GetModuleConfigurationQuery("application", "module");
            var queryHandler = this.CreateQueryHandler(repository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.IsNotNull(messageCollection);
            Assert.IsTrue(messageCollection.HasErrors);
            Assert.AreEqual(1, messageCollection.AllMessages.Count());
            StringAssert.Contains("Application Configuration not found", messageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenValidConfiguration_ShouldReturnModuleConfiguration()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "My Halo";
            const string moduleName      = "Home";
            var repository               = new TileConfigurationRepository(this.iocContainer);
            var queryHandler             = this.CreateQueryHandler(repository);
            var query                    = new GetModuleConfigurationQuery(applicationName, moduleName, returnAllRoots: true);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            var applicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            var moduleConfiguration      = repository.FindModuleConfiguration(applicationConfiguration, moduleName);

            Assert.Greater(query.Result.Results.Count(), 0);
            Assert.AreEqual(moduleConfiguration.Name, query.Result.Results.First().ModuleConfiguration.Name);
        }

        private GetModuleConfigurationQueryHandler CreateQueryHandler(ITileConfigurationRepository tileConfigurationRepository = null)
        {
            var configurationRepository = tileConfigurationRepository ?? new TileConfigurationRepository(this.iocContainer);
            var queryHandler            = new GetModuleConfigurationQueryHandler(configurationRepository);
            return queryHandler;
        }
    }
}
