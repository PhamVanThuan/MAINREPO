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
    public class ChildTileConfigTests : HaloUIConfigTests
    {
        [Test]
        public void ChildTileConfigurationTest()
        {
            //---------------Set up test pack-------------------
            var childTiles = this.childTile.GetRegisteredTypes();
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "ChildTile: {0} with RootTile: {1}";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsAnyRootTilePredicate>((expectedTileConfig) => {
                var expectedRootTileConfigName = String.Format(rootTileConfSuffix, expectedTileConfig.Name);
                var expected = String.Empty;
                var actual = String.Empty;
                var expectedChildTileNames = expectedTileConfig.ChildTiles.Select(y => String.Format(childTileConfSuffix, y.Name)).ToArray();
                var matchingActualChildTileTypes = childTiles.Where(x => expectedChildTileNames.Contains(x.Name));
                var matchingActualChildTiles = matchingActualChildTileTypes.Select(x => x.Name).ToArray();
                var differenceQuery = expectedChildTileNames.Except(matchingActualChildTiles);
                foreach (var expectedChildTileName in differenceQuery)
                {
                    if (!configurations.ContainsKey(expectedChildTileName))
                    {
                        expected = String.Format(messageTemplate, expectedChildTileName, expectedRootTileConfigName);
                        configurations.Add(expected, actual);
                    }
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }


        [Test]
        public void ChildTileNotLinkedToAlternativeRootTiles()
        {
            //---------------Set up test pack-------------------
            var childTiles = this.childTile.GetRegisteredTypes();
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "ChildTile: {0} to not be linked to AlternativeRootTile: {1}";
            var messageTemplate2 = "ChildTile: {0} is linked to AlternativeRootTile: {1}";
            var actual = String.Empty;
            var expected = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            foreach (var childTileType in childTiles)
            {
                var childTileTypeInterface = childTileType.GetInterfaces().Where(x => x.Name.Contains(typeof(IHaloChildTileConfiguration).Name)).First();
                var childTileGenericType = childTileTypeInterface.GetGenericArguments().FirstOrDefault();

                var isLinkedToAlternativeRootTile = childTileGenericType.GetInterfaces().Where(x => x == typeof(IHaloAlternativeRootTileConfiguration)).Any();
                if (isLinkedToAlternativeRootTile)
                {
                    expected = String.Format(messageTemplate, childTileType.Name, childTileGenericType.Name);
                    actual = String.Format(messageTemplate2, childTileType.Name, childTileGenericType.Name);
                    configurations.Add(expected, actual);
                }
            }

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }
    }
}
