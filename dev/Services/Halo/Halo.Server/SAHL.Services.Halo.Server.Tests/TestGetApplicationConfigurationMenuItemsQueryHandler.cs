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
    public class TestGetApplicationConfigurationMenuItemsQueryHandler : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var tileConfigurationRepository = Substitute.For<ITileConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryHandler = new GetApplicationConfigurationMenuItemsQueryHandler(tileConfigurationRepository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryHandler);
        }

        [Test]
        public void Constructor_GivenNullTileConfigurationRepository_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetApplicationConfigurationMenuItemsQueryHandler(null));
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
            var queryHandler            = new GetApplicationConfigurationMenuItemsQueryHandler(repository);
            var query                   = new GetApplicationConfigurationMenuItemsQuery("application", null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => systemMessageCollection = queryHandler.HandleQuery(query));
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsTrue(systemMessageCollection.HasErrors);
            Assert.AreEqual(1, systemMessageCollection.AllMessages.Count());
            StringAssert.Contains(testExceptionMessage, systemMessageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenValidConfiguration_ShouldReturnAllConfiguredApplications()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "My Halo";

            var repository   = new TileConfigurationRepository(this.iocContainer);
            var queryHandler = new GetApplicationConfigurationMenuItemsQueryHandler(repository);
            var query        = new GetApplicationConfigurationMenuItemsQuery(applicationName, null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            var applicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            Assert.IsNotNull(applicationConfiguration);

            var expectedMenuItems = repository.FindAllMenuItemsForApplication(applicationConfiguration);
            Assert.AreEqual(expectedMenuItems.Count(), query.Result.Results.Count());
        }
    }
}
