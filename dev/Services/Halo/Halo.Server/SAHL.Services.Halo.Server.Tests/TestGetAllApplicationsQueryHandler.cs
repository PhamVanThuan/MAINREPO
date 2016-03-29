using System;
using System.Linq;

using StructureMap;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using SAHL.UI.Halo.Shared;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestGetAllApplicationsQueryHandler
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression => expression.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                    scanner.LookForRegistries();
                }));

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var tileConfigurationRepository = Substitute.For<ITileConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryHandler = new GetAllApplicationsQueryHandler(tileConfigurationRepository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryHandler);
        }

        [Test]
        public void Constructor_GivenNullTileConfigurationRepository_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetAllApplicationsQueryHandler(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("tileConfigurationRepository", exception.ParamName);
        }

        [Test]
        public void HandleQuery_GivenRepositoryThrowsException_ShouldReturnExceptionDetailsInSystemCollection()
        {
            //---------------Set up test pack-------------------
            const string testExceptionMessage = "Test exception message";
            var repository                    = Substitute.For<ITileConfigurationRepository>();

            repository.FindAllApplicationConfigurations().Returns(info =>
                {
                    throw new Exception(testExceptionMessage);
                });

            var systemMessageCollection = SystemMessageCollection.Empty();
            var queryHandler            = new GetAllApplicationsQueryHandler(repository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => systemMessageCollection = queryHandler.HandleQuery(new GetAllApplicationsQuery()));
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
            var repository              = new TileConfigurationRepository(this.iocContainer);
            var queryHandler            = this.CreateQueryHandler(repository);
            var getAllApplicationsQuery = new GetAllApplicationsQuery();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            queryHandler.HandleQuery(getAllApplicationsQuery);
            //---------------Test Result -----------------------
            var expectedApplications = repository.FindAllApplicationConfigurations();
            Assert.AreEqual(expectedApplications.Count(), getAllApplicationsQuery.Result.Results.Count());
        }

        private GetAllApplicationsQueryHandler CreateQueryHandler(ITileConfigurationRepository tileConfigurationRepository = null)
        {
            var configurationRepository = tileConfigurationRepository ?? new TileConfigurationRepository(this.iocContainer);
            var queryHandler            = new GetAllApplicationsQueryHandler(configurationRepository);
            return queryHandler;
        }
    }
}
