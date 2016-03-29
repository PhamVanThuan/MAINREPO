using NUnit.Framework;
using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.Core.Logging;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Config;
using SAHL.Core.Testing.Config.UI;
using SAHL.Core.Testing.Factories;
using SAHL.Core.Testing.Providers;
using SAHL.UI.Halo.Shared.Configuration;
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
    public class ConfigNotInSyncWithJson : HaloUIConfigTests
    {
        [Test]
        public void ConfigurationsNotInJsonConfigTest()
        {
            //---------------Set up test pack-------------------
            var childTiles = this.childTile.GetRegisteredTypes();
            var rootTiles = this.rootTiles.GetRegisteredTypes();
            var tileActions = this.tileActions.GetRegisteredTypes();
            var drillDownTilesAndPages = this.tileOrPageDrilldowns.GetRegisteredTypes();
            var haloModules = this.haloModuleTiles.GetRegisteredTypes();
            var linkedRootTiles = this.linkedRootTiles.GetRegisteredTypes();

            var configurations = new Dictionary<string, string>();
            var actual = String.Empty;
            var expected = String.Empty;
            var messageTemplate1 = "{0} configuration to be in HaloUIConfig.json";
            var messageTemplate2 = "Located {0} configuration in Halo";

            var expectedRootTileConfigNames = this.haloUIConfigTree.Traverse().Where(x => x.Type == RootTile).Select(x => String.Format(rootTileConfSuffix, x.Name));
            var expectedChildTileConfigNames = this.haloUIConfigTree.Traverse().Where(x => x.Type == ChildTile).Select(x => String.Format(childTileConfSuffix, x.Name));
            var expectedTileActionConfigNames = this.haloUIConfigTree.Traverse().Where(x => x.Type == TileAction).Select(x => String.Format(tileActionConfSuffix, x.Name));
            var expectedTileDrilldownConfigNames = this.haloUIConfigTree.Traverse().Where(x => x.HasChildTileDrilldown).Select(x => String.Format(tileOrPageDrilldownConfSuffix, x.Name, x.Type));
            var expectedPageDrilldownConfigNames = this.haloUIConfigTree.Traverse().Where(x => x.HasChildPageDrilldown).Select(x => String.Format(tileOrPageDrilldownConfSuffix, x.Name, ChildPage));
            var expectedHaloModuleConfigNames = this.haloUIConfigTree.Traverse().Where(x => x.Type == HaloModule).Select(x => String.Format(haloModuleConfSuffix, x.Name, HaloModule));
            var expectedLinkedRootTileConfigNames = this.haloUIConfigTree.Traverse().Where(x => x.Type == LinkedRootTile).Select(x => String.Format(linkedRootTileConfSuffix, x.Name, LinkedRootTile));

            var actualRootTileConfigNames = rootTiles.Select(x => x.Name);
            var actualChildTileConfigNames = childTiles.Select(x => x.Name);
            var actualTileActionConfigNames = tileActions.Select(x => x.Name);
            var actualTileDrilldownConfigNames = drillDownTilesAndPages.Where(x => x.Name.Contains(ChildTile)).Select(x => x.Name);
            var actualPageDrilldownConfigNames = drillDownTilesAndPages.Where(x => x.Name.Contains(ChildPage)).Select(x => x.Name);
            var actualHaloModuleConfigNames = haloModules.Where(x => x.Name.Contains(HaloModule)).Select(x => x.Name);
            var actualLinkedRootTileConfigNames = linkedRootTiles.Where(x => x.Name.Contains(LinkedRootTile)).Select(x => x.Name);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var configurationDifferences = new List<string>();
            configurationDifferences.AddRange(actualRootTileConfigNames.Except(expectedRootTileConfigNames));
            configurationDifferences.AddRange(actualChildTileConfigNames.Except(expectedChildTileConfigNames));
            configurationDifferences.AddRange(actualTileActionConfigNames.Except(expectedTileActionConfigNames));
            configurationDifferences.AddRange(actualTileDrilldownConfigNames.Except(expectedTileDrilldownConfigNames));
            configurationDifferences.AddRange(actualPageDrilldownConfigNames.Except(expectedPageDrilldownConfigNames));
            configurationDifferences.AddRange(actualHaloModuleConfigNames.Except(expectedHaloModuleConfigNames));
            configurationDifferences.AddRange(actualLinkedRootTileConfigNames.Except(expectedLinkedRootTileConfigNames));

            foreach (var configurationItem in configurationDifferences)
            {
                expected = String.Format(messageTemplate1, configurationItem);
                actual = String.Format(messageTemplate2, configurationItem);
                configurations.Add(expected, actual);
            }
          

          //---------------Test Result -----------------------
          AssertConfiguration(configurations);

        }
    }
}
