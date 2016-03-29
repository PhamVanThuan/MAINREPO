using System;
using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.Caching;
using SAHL.UI.Halo.Shared.Repository;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestIocRegistry
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

            var whatDoIHave = ObjectFactory.WhatDoIHave();

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void GetInstance_GivenTileConfigurationRepository_ShouldReturnRepository()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = ObjectFactory.GetInstance<ITileConfigurationRepository>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
            Assert.IsInstanceOf<TileConfigurationRepository>(repository);
        }

        [Test]
        public void GetInstance_GivenTileDataRepository_ShouldReturnRepository()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = ObjectFactory.GetInstance<ITileDataRepository>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
            Assert.IsInstanceOf<TileDataRepository>(repository);
        }

        [Test]
        public void GetInstance_GivenTileWizardConfigurationRepository_ShouldReturnRepository()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = ObjectFactory.GetInstance<ITileWizardConfigurationRepository>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
            Assert.IsInstanceOf<TileWizardConfigurationRepository>(repository);
        }

        [Test]
        public void GetInstance_GivenConfigurationCacheManagerRepository_ShouldReturnCacheManager()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = ObjectFactory.GetInstance<IHaloConfigurationCacheManager>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
            Assert.IsInstanceOf<HaloConfigurationCacheManager>(repository);
        }

        [Test]
        public void GetInstance_GivenWorkflowTileActionProviderInterface_ShouldReturnProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = ObjectFactory.GetInstance<IHaloWorkflowTileActionProvider>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
            Assert.IsInstanceOf<HaloWorkflowTileActionProvider>(provider);
        }

        [Test]
        public void GetInstance_GivenWorkflowTileActionProviderType_ShouldReturnProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = ObjectFactory.GetInstance(typeof(HaloWorkflowTileActionProvider));
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
            Assert.IsInstanceOf<IHaloWorkflowTileActionProvider>(provider);
        }
    }
}
