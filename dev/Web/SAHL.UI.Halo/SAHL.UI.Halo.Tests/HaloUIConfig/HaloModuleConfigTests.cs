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
    public class HaloModuleConfigTests : HaloUIConfigTests
    {
        [Test]
        public void AllHaloModuleConfigurationsTest()
        {
             //---------------Set up test pack-------------------
            var moduleTiles = this.haloModuleTiles.GetRegisteredTypes();
            var rootTiles = this.rootTiles.GetRegisteredTypes();
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "HaloModule: {0}";
            var actual = String.Empty;
            var expected = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsAnyHaloModulePredicate>((expectedTileConfig) =>
            {
                var expectedModuleTileConfigName = String.Format(haloModuleConfSuffix, expectedTileConfig.Name);
                var moduleTileType = moduleTiles.Where(x => x.Name == expectedModuleTileConfigName).FirstOrDefault();
                if (moduleTileType == null)
                {
                    expected = String.Format(messageTemplate, expectedModuleTileConfigName);
                    actual = String.Format(messageTemplate, "MISSING / INCORRECTLY NAMED");
                    configurations.Add(expected, actual);
                }
                else
                {
                    foreach (var rootTileConfig in expectedTileConfig.RootTiles)
                    {
                        var messageTemplate2 = "RootTile: {0}";
                        actual = String.Empty;
                        expected = String.Empty;

                        var expectedRootTileConfigName = String.Format(rootTileConfSuffix, rootTileConfig.Name);
                        var rootTileType = rootTiles.Where(x => x.Name == expectedRootTileConfigName).FirstOrDefault();
                        if (rootTileType != null)
                        {
                            var rootTileInterfaces = rootTileType.GetInterfaces();
                            var rootTileTypeInterface = rootTileInterfaces.Where(x => x.Name.Contains(typeof(IHaloModuleTileConfiguration<>).Name)).FirstOrDefault();
                            if (rootTileTypeInterface == null)
                            {
                                expected = String.Format(messageTemplate2, String.Format("{0} to have ModuleTileConfiguration.", expectedRootTileConfigName));
                                actual = String.Format(messageTemplate2, String.Format("{0} does not have any ModuleTileConfiguration.", expectedRootTileConfigName));
                                configurations.Add(expected, actual);
                            }
                            else
                            {
                                var actualRootTileModuleType = rootTileTypeInterface.GetGenericArguments().FirstOrDefault();
                                var expectedRootTileModuleType = moduleTileType;

                                if (actualRootTileModuleType != expectedRootTileModuleType)
                                {
                                    expected = String.Format(messageTemplate2, String.Format(" {0} to be linked to {1}",expectedRootTileConfigName, moduleTileType));
                                    actual = String.Format(messageTemplate2, String.Format("{0} is currently linked to {1}", expectedRootTileConfigName,actualRootTileModuleType));
                                    configurations.Add(expected, actual);
                                }
                            }
                        }
                    }
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }
    }
}
