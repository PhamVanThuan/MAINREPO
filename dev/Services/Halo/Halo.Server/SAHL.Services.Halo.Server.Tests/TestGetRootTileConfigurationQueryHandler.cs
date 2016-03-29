using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Logging;
using SAHL.UI.Halo.Shared;
using SAHL.Core.Data.Context;
using SAHL.Core.BusinessModel;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.SystemMessages;
using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.Services.Interfaces.Halo.Queries;
using SAHL.Services.Halo.Server.QueryHandlers;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestGetRootTileConfigurationQueryHandler : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var tileConfigurationRepository = Substitute.For<ITileConfigurationRepository>();
            var tileDataRepository          = Substitute.For<ITileDataRepository>();
            var rawLogger                   = Substitute.For<IRawLogger>();
            var loggerSource                = Substitute.For<ILoggerSource>();
            var loggerAppSource             = Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryHandler = new GetRootTileConfigurationQueryHandler(tileConfigurationRepository, tileDataRepository, 
                                                                        rawLogger, loggerSource, loggerAppSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryHandler);
        }

        [TestCase("tileConfigurationRepository")]
        [TestCase("tileDataRepository")]
        [TestCase("rawLogger")]
        [TestCase("loggerSource")]
        [TestCase("loggerAppSource")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<GetRootTileConfigurationQueryHandler>(parameterName);
            //---------------Test Result -----------------------
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
            var queryHandler            = this.CreateQueryHandler(repository);
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext         = new BusinessContext("context", GenericKeyType.Account, 0);
            var query                   = new GetRootTileConfigurationQuery("application", "module", "root tile", businessContext, haloRoleModel);
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
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            var query           = new GetRootTileConfigurationQuery("application", "module", "root tile", businessContext, haloRoleModel);
            var queryHandler    = this.CreateQueryHandler();
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
        public void HandleQuery_GivenModuleConfigurationNotFound_ShouldReturnErrorInMessageCollection()
        {
            //---------------Set up test pack-------------------
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            var query           = new GetRootTileConfigurationQuery("Home", "module", "root tile", businessContext, haloRoleModel);
            var queryHandler    = this.CreateQueryHandler();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.IsNotNull(messageCollection);
            Assert.IsTrue(messageCollection.HasErrors);
            Assert.AreEqual(1, messageCollection.AllMessages.Count());
            StringAssert.Contains("Module Configuration not found", messageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenTileConfigurationNotFound_ShouldReturnErrorInMessageCollection()
        {
            //---------------Set up test pack-------------------
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            var query           = new GetRootTileConfigurationQuery("Home", "Clients", "root tile", businessContext, haloRoleModel);
            var queryHandler    = this.CreateQueryHandler();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.IsNotNull(messageCollection);
            Assert.IsTrue(messageCollection.HasErrors);
            Assert.AreEqual(1, messageCollection.AllMessages.Count());
            StringAssert.Contains("Tile Configuration not found", messageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenRootTileConfigurationNotFound_ShouldReturnErrorInMessageCollection()
        {
            //---------------Set up test pack-------------------
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            var query           = new GetRootTileConfigurationQuery("Home", "Clients", "Legal Entity Address", businessContext, haloRoleModel);
            var queryHandler    = this.CreateQueryHandler();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.IsNotNull(messageCollection);
            Assert.IsTrue(messageCollection.HasErrors);
            Assert.AreEqual(1, messageCollection.AllMessages.Count());
            StringAssert.Contains("Root Tile Configuration not found", messageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenValidConfiguration_ShouldReturnRootTileConfiguration()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            const string moduleName      = "Clients";
            const string rootTileName    = "Client";
            var haloRoleModel            = new HaloRoleModel("area", "name", new string[]{ "capability"});
            var queryHandler             = this.CreateQueryHandler();
            var query                    = new GetRootTileConfigurationQuery(applicationName, moduleName, rootTileName, 
                                                                             new BusinessContext("context", GenericKeyType.Account, 0), haloRoleModel);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.IsFalse(messageCollection.HasErrors);
            Assert.IsNotNull(query.Result);
            Assert.Greater(query.Result.Results.Count(), 0);

            var rootTileConfiguration = new ClientRootTileConfiguration();

            Assert.AreEqual(rootTileConfiguration.Name, query.Result.Results.First().RootTileConfiguration.RootTileConfigurations.First().Name);
        }

        [Test]
        public void HandleQuery_GivenValidConfiguration_ShouldReturnRootTileConfigurationWithTileActions()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            const string moduleName      = "Clients";
            const string rootTileName    = "Client";
            var queryHandler             = this.CreateQueryHandler();
            var haloRoleModel            = new HaloRoleModel("area", "name", new string[]{ "capabilities"});
            var businessContext          = new BusinessContext("context", GenericKeyType.Account, 0);
            var query                    = new GetRootTileConfigurationQuery(applicationName, moduleName, rootTileName, businessContext, haloRoleModel);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            var rootTileConfiguration = query.Result.Results.First().RootTileConfiguration.RootTileConfigurations.First();
            Assert.IsNotNull(rootTileConfiguration);
        }

        [Test]
        public void HandleQuery_GivenDynamicRootTileConfiguration_ShouldReturnAlternative()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            const string moduleName      = "Clients";
            const string rootTileName    = "Application";
            var queryHandler             = this.CreateQueryHandler();
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext          = new BusinessContext("context", GenericKeyType.Account, 0);
            var query                    = new GetRootTileConfigurationQuery(applicationName, moduleName, rootTileName, businessContext, haloRoleModel);

            var tileDataModel = new HaloDynamicTileDataModel
                {
                    GenericKey        = 1,
                    GenericKeyTypeKey = 1,
                    SubType           = "New Purchase Loan"
                };
            var models = new List<HaloDynamicTileDataModel> { tileDataModel };
            this.UpdateDbFactoryWithQueryResult(models);

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            this.ResetDbFactoryToDefault();
            //---------------Test Result -----------------------
            Assert.IsFalse(messageCollection.HasErrors);
            Assert.IsFalse(messageCollection.HasWarnings);

            var rootTileConfiguration = query.Result.Results.First().RootTileConfiguration.RootTileConfigurations.First();

            Assert.IsNotNull(rootTileConfiguration);
            Assert.AreEqual("New Purchase Application", rootTileConfiguration.Name);
        }

        private GetRootTileConfigurationQueryHandler CreateQueryHandler(ITileConfigurationRepository tileConfigurationRepository = null,
                                                                        ITileDataRepository tileDataRepository = null)
        {
            var rawLogger       = Substitute.For<IRawLogger>();
            var loggerSource    = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();

            var configurationRepository = tileConfigurationRepository ?? new TileConfigurationRepository(this.iocContainer);
            var dataRepository          = tileDataRepository ?? new TileDataRepository(this.iocContainer);

            var queryHandler = new GetRootTileConfigurationQueryHandler(configurationRepository, dataRepository, rawLogger, loggerSource, loggerAppSource);
            return queryHandler;
        }

        private void UpdateDbFactoryWithQueryResult(dynamic expectedResult)
        {
            var dbFactory         = Substitute.For<IDbFactory>();
            var db                = Substitute.For<IDb>();
            var readOnlyDbContext = Substitute.For<IReadOnlyDbContext>();

            readOnlyDbContext.Select<HaloDynamicTileDataModel>(Arg.Any<string>()).Returns(info => expectedResult);
            db.InReadOnlyAppContext().Returns(readOnlyDbContext);
            dbFactory.NewDb().Returns(db);

            this.iocContainer.Inject(dbFactory);
        }

        private void ResetDbFactoryToDefault()
        {
            this.iocContainer.Inject(new FakeDbFactory());
        }
    }
}
