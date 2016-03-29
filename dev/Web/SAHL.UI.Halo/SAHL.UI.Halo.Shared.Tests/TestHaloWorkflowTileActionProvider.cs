using NSubstitute;
using NUnit.Framework;
using SAHL.Core;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.Caching;
using SAHL.UI.Halo.Shared.Models;
using StructureMap;
using System;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloWorkflowTileActionProvider
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                            scanner.LookForRegistries();
                        });

                    expression.For<IDbFactory>().Use<FakeDbFactory>();
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var dbFactory = Substitute.For<IDbFactory>();
            var cacheManager = Substitute.For<IHaloConfigurationCacheManager>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = new HaloWorkflowTileActionProvider(dbFactory, cacheManager);
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
        }

        [TestCase("dbFactory")]
        [TestCase("cacheManager")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<HaloWorkflowTileActionProvider>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void GetTileActions_GivenNullBusinessContext_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateProvider();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => provider.GetTileActions(null, string.Empty, new string[0]));
            //---------------Test Result -----------------------
            Assert.AreEqual("businessContext", exception.ParamName);
        }

        [Test]
        public void GetTileActions_GivenBusinessContext_ShouldCallDbWorkflowContextWithUiStatement()
        {
            //---------------Set up test pack-------------------
            var fakeDbFactory = new FakeDbFactory();
            var dbContext = fakeDbFactory.FakedDb.InWorkflowContext();

            var processInfoModel = new X2ProcessInfoModel { ID = 1, Name = "Process" };
            var processInfoModels = new List<X2ProcessInfoModel> { processInfoModel };
            fakeDbFactory.FakedDb.DbContext.Select<X2ProcessInfoModel>(Arg.Any<string>()).Returns(processInfoModels);

            var provider = this.CreateProvider(fakeDbFactory);
            var businessContext = new BusinessContext(string.Empty, GenericKeyType.Account, 1234);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => provider.GetTileActions(businessContext, @"SAHL\\JessicaV", new string[]{"Invoice Processor"}));
            //---------------Test Result -----------------------
            var expectedString = provider.GetUiStatementToLoadWorkflow(businessContext);
            dbContext.Received(1).Select<WorkFlowDataModel>(expectedString);
        }

        [Test]
        public void GetTileActions_GivenAnExceptionIsThrownInDbContext_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var fakeDbFactory = new FakeDbFactory();
            fakeDbFactory.FakedDb.DbContext.Select<WorkFlowDataModel>(Arg.Any<string>()).Returns(info =>
                {
                    throw new Exception("Test Exception");
                });

            var provider = this.CreateProvider(fakeDbFactory);
            var businessContext = new BusinessContext(string.Empty, GenericKeyType.Account, 1234);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActions = provider.GetTileActions(businessContext, string.Empty,new string[0]);
            //---------------Test Result -----------------------
            Assert.IsNull(tileActions);
        }

        private IHaloWorkflowTileActionProvider CreateProvider(IDbFactory dbFactory = null,
                                                               IHaloConfigurationCacheManager cacheManager = null)
        {
            dbFactory = dbFactory ?? Substitute.For<IDbFactory>();
            cacheManager = cacheManager ?? new HaloConfigurationCacheManager();

            var provider = new HaloWorkflowTileActionProvider(dbFactory, cacheManager);
            return provider;
        }
    }
}
