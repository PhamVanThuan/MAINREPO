using NUnit.Framework;
using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.Core.Logging;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Config;
using SAHL.Core.Testing.Config.UI;
using SAHL.Core.Testing.Factories;
using SAHL.Core.Testing.FileConventions;
using SAHL.Core.Testing.Ioc;
using SAHL.Core.Testing.Ioc.Registration;
using SAHL.Core.Testing.Providers;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Tests.Conventions;
using SAHL.UI.Halo.Tests.HaloUIConfigPredicates;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace SAHL.UI.Halo.Tests.HaloUIConfig
{
    [TestFixture]
    public abstract class HaloUIConfigTests
    {
        protected ITestingIocContainer rootTiles;
        protected ITestingIocContainer childTile;
        protected ITestingIocContainer tileActions;
        protected ITestingIocContainer tileOrPageDrilldowns;
        protected ITestingIocContainer contentDataProviders;
        protected ITestingIocContainer dataProviderTypes;
        protected ITestingIocContainer alternativeRootTiles;
        protected ITestingIocContainer dynamicDataProviderTypes;
        protected ITestingIocContainer dynamicRootTiles;
        protected ITestingIocContainer haloTileModels;
        protected ITestingIocContainer haloModuleTiles;
        protected ITestingIocContainer linkedRootTiles;

        protected ITestingIoc testingIoc;
        protected HaloUIConfigTree haloUIConfigTree;
        protected IDbFactory dbFactory;

        protected const string tileActionConfSuffix = "{0}Action";
        protected const string rootTileConfSuffix = "{0}RootTileConfiguration";
        protected const string linkedRootTileConfSuffix = "{0}LinkedRootTileConfiguration";
        protected const string haloModuleConfSuffix = "{0}Configuration";
        protected const string childTileConfSuffix = "{0}ChildTileConfiguration";
        protected const string tileOrPageDrilldownConfSuffix = "{0}{1}Drilldown";
        protected const string dataContentProviderConfSuffix = "{0}{1}ContentDataProvider";
        protected const string dataProviderConfSuffix = "{0}{1}DataProvider";
        protected const string ChildTile = "ChildTile";
        protected const string RootTile = "RootTile";
        protected const string ChildPage = "ChildPage";
        protected const string TileAction = "TileAction";
        protected const string TileDrilldown = "TileDrilldown";
        protected const string HaloModule = "HaloModule";
        protected const string LinkedRootTile = "LinkedRootTile";
        

        [TestFixtureSetUp]
        public void Setup()
        {
            var haloUIConfigFactory = new HaloUIConfigFactory();
            var haloUIConfigPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "HaloUIConfig\\HaloUIConfig.json");
            var haloUIConfig = haloUIConfigFactory.Create(haloUIConfigPath);
            
            this.haloUIConfigTree = new HaloUIConfigTree(haloUIConfig);
           
            this.testingIoc = TestingIoc.Initialise();
            this.dbFactory = testingIoc.GetInstance<IDbFactory>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloRootTileConfigTestConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloChildTileConfigTestConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloAlternativeRootTileConfigTestConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloTileDrilldownActionConfigTestConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloTileActionConfigTestConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloTileContentDataProviderConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloTileDataProviderConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloTileDynamicDataProviderConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloDynamicRootTileConfigTestConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloTileModelConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloModuleConfigTestConvention>();
            this.testingIoc.Configure<HaloUIAssemblyConvention, HaloRootTileLinkedConfigConvention>();

            this.rootTiles = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloRootTileConfigTestConvention>>();
            this.childTile = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloChildTileConfigTestConvention>>();
            this.alternativeRootTiles = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloAlternativeRootTileConfigTestConvention>>();
            this.tileOrPageDrilldowns = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloTileDrilldownActionConfigTestConvention>>();
            this.tileActions = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloTileActionConfigTestConvention>>();
            this.contentDataProviders = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloTileContentDataProviderConvention>>();
            this.dataProviderTypes = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloTileDataProviderConvention>>();
            this.dynamicDataProviderTypes = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloTileDynamicDataProviderConvention>>();
            this.dynamicRootTiles = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloDynamicRootTileConfigTestConvention>>();
            this.haloTileModels = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloTileModelConvention>>();
            this.haloModuleTiles = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloModuleConfigTestConvention>>();
            this.linkedRootTiles = this.testingIoc.GetInstance<ITestingIocContainer<HaloUIAssemblyConvention, HaloRootTileLinkedConfigConvention>>();
        }

        protected void ExecuteDataProvider(Dictionary<string, string> expectedProviderConfigurations, Type dataProviderType, Action dataProviderTest)
        {
            var messageTemplate = "DataProvider: {0}";
            try
            {
                dataProviderTest.Invoke();
            }
            catch (Exception e)
            {
                var expected = String.Format(messageTemplate, dataProviderType.Name);
                expected = String.Format("{0} to load without error.", expected);
                var actual = String.Format(messageTemplate, "but failed with error: {0}");
                actual = String.Format(actual, e.Message);
                if (!expectedProviderConfigurations.ContainsKey(expected))
                {
                    expectedProviderConfigurations.Add(expected, actual);
                }
            }
        }
        protected void DataProviderTests(HaloUIConfigItem expectedConfig, Dictionary<string, string> expectedProviderConfigurations, string suffix, out string expectedProviderConfigName, ITestingIocContainer dataProviderContainer)
        {
            var messageTemplate = "DataProvider: {0}";
            string actual, expected = String.Empty;
            var dataProviders = dataProviderContainer.GetRegisteredTypes();
            expectedProviderConfigName = String.Format(suffix, expectedConfig.Name, expectedConfig.Type);
            var expectedProviderConfigNameLambda = expectedProviderConfigName;
            if (!String.IsNullOrEmpty(expectedConfig.CommonDataProviderName))
            {
                expectedProviderConfigNameLambda = expectedConfig.CommonDataProviderName;
            }
            var matchingActualProvidersTypes = dataProviders.Where(x => expectedProviderConfigNameLambda == x.Name).ToArray();
            var matchingActualProvidersNames = matchingActualProvidersTypes.Select(x => x.Name).ToArray();
            actual = String.Empty;
            expected = String.Format(messageTemplate, expectedProviderConfigName);
            if (matchingActualProvidersNames.Count() == 0)
            {
                actual = String.Format(messageTemplate, "MISSING / INCORRECTLY NAMED");
                expectedProviderConfigurations.Add(expected, actual);
            }
            if (matchingActualProvidersNames.Count() > 1)
            {
                actual = String.Format(messageTemplate, "DUPLICATED");
                expectedProviderConfigurations.Add(expected, actual);
            }
        }
        protected void AssertConfiguration(Dictionary<string, string> incorrectConfig)
        {
            var incorrectConfigClone = (IEnumerable<KeyValuePair<string, string>>)incorrectConfig.ToArray().Clone();
            foreach (var item in incorrectConfigClone)
            {
                if (item.Key == item.Value)
                {
                    incorrectConfig.Remove(item.Key);
                }
            }
            var messageTemplate = "\r\n Incorrect Configuration: \r\n -> Expected: {0} \r\n -> Actual: {1}";
            var messages = String.Empty;
            foreach (var item in incorrectConfig)
            {
                messages += String.Format(messageTemplate, item.Key, item.Value);
            }

            Assert.AreEqual(0, incorrectConfig.Count, messages);
        }
    }
}
