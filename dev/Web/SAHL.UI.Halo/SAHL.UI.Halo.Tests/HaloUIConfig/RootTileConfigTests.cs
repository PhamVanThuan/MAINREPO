using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration;
using SAHL.UI.Halo.Tests.HaloUIConfigPredicates;

namespace SAHL.UI.Halo.Tests.HaloUIConfig
{
    [TestFixture]
    public class RootTileConfigTests : HaloUIConfigTests
    {
        [Test]
        public void AllRootTileConfigurationsTest()
        {
            //---------------Set up test pack-------------------
            var rootTiles = this.rootTiles.GetRegisteredTypes();
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "RootTile: {0}";
            var actual = String.Empty;
            var expected = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsAnyRootTilePredicate>((expectedTileConfig) =>
            {
                var expectedRootTileConfigName = String.Format(rootTileConfSuffix, expectedTileConfig.Name);
                var matchingActualRootTiles = rootTiles.Where(x => expectedRootTileConfigName == x.Name).Select(x => x.Name).ToArray();
                actual = String.Empty;
                if (matchingActualRootTiles.Count() == 0)
                {
                    actual = String.Format(messageTemplate, "MISSING / INCORRECTLY NAMED");
                }
                if (matchingActualRootTiles.Count() > 1)
                {
                    actual = String.Format(messageTemplate, "DUPLICATED");
                }
                if (!String.IsNullOrEmpty(actual))
                {
                    expected = String.Format(messageTemplate, expectedRootTileConfigName);
                    configurations.Add(expected, actual);
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void AlternativeRootTilesConfiguredToDynamicRootTilesTest()
        {
            //---------------Set up test pack-------------------
            var alternativeRootTiles = this.alternativeRootTiles.GetRegisteredTypes();
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "AlternativeRootTile configured to DynamicRootTile: {0}";
            var actual = String.Empty;
            var expected = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsAlternativeRootTilePredicate>((expectedTileConfig) =>
            {
                if (expectedTileConfig.DynamicTile.Name == null)
                {
                    actual = String.Format(messageTemplate, "null");
                    expected = String.Format(messageTemplate, "not null");
                    configurations.Add(expected, actual);
                }
                else
                {

                    var alternativeRootTileConfigName = String.Format(rootTileConfSuffix, expectedTileConfig.Name);
                    var dynamicRootTileConfigName = String.Format(rootTileConfSuffix, expectedTileConfig.DynamicTile.Name);

                    var alternativeType = alternativeRootTiles.Where(x => x.Name == alternativeRootTileConfigName).First();
                    var alternativeTypeInterface = alternativeType.GetInterfaces().Where(x => x.Name.Contains(typeof(IHaloAlternativeRootTileConfiguration).Name)).First();
                    var alternativeGenericType = alternativeTypeInterface.GetGenericArguments().FirstOrDefault();


                    if (alternativeGenericType.Name != dynamicRootTileConfigName)
                    {
                        actual = String.Format(messageTemplate, alternativeGenericType.Name);
                        expected = String.Format(messageTemplate, dynamicRootTileConfigName);
                        configurations.Add(expected, actual);
                    }
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void AtLeast2AlternativeRootTilesPerDynamicRootTileTest()
        {
            //-----------------------Setup Test Pack----------------------------
            var alternativeRootTiles = this.alternativeRootTiles.GetRegisteredTypes();
            var dynamicTiles = this.dynamicRootTiles.GetRegisteredTypes();

            var configurations = new Dictionary<string, string>();
            var actual = String.Empty;
            var expected = String.Empty;
            var messageTemplate1 = "DynamicTile: {0} to have at least two AlternativeRootTiles";
            var messageTemplate2 = "DynamicTile: {0} only have {1} AlternativeRootTiles";

            //-----------------------Assert Precondition------------------------

            //-----------------------Execute Test-------------------------------
            foreach (var dynamicTile in dynamicTiles)
            {
                var alternativeRootTileCount = 0;
                foreach (var alternativeRootTile in alternativeRootTiles)
                {
                    var alternativeRootTileInterfaces = alternativeRootTile.GetInterfaces();
                    var alternativeRootTileInterface = alternativeRootTileInterfaces.Where(x => x.Name == "IHaloAlternativeRootTileConfiguration`1").First();
                    var alternativeRootTileDynamicTile = alternativeRootTileInterface.GetGenericArguments().First();

                    if (dynamicTile == alternativeRootTileDynamicTile)
                    {
                        alternativeRootTileCount++;
                    }
                }

                if (alternativeRootTileCount < 2)
                {
                    expected = String.Format(messageTemplate1, dynamicTile.Name);
                    actual = String.Format(messageTemplate2, dynamicTile.Name, alternativeRootTileCount);
                    configurations.Add(expected, actual);
                }
            }

            //-----------------------Test Result--------------------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void RootTilesLinkedToModuleTilesTest()
        {
             //---------------Set up test pack-------------------
            var rootTiles = this.rootTiles.GetRegisteredTypes();
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "RootTile: {0}";
            var actual = String.Empty;
            var expected = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsAnyRootTilePredicate>((expectedTileConfig) =>
            {
                var expectedRootTileConfigName = String.Format(rootTileConfSuffix, expectedTileConfig.Name);
                var rootTileType = rootTiles.Where(x => x.Name == expectedRootTileConfigName).FirstOrDefault();
                if (rootTileType != null)
                {
                    var rootTileInterfaces = rootTileType.GetInterfaces();
                    var rootTileTypeInterface = rootTileInterfaces.Where(x => x.Name.Contains(typeof(IHaloModuleTileConfiguration<>).Name)).FirstOrDefault();
                    if (rootTileTypeInterface == null)
                    {
                        expected = String.Format(messageTemplate, String.Format("{0} to have ModuleTileConfiguration.", expectedRootTileConfigName));
                        actual = String.Format(messageTemplate, String.Format("{0} does not have any ModuleTileConfiguration.", expectedRootTileConfigName));
                        configurations.Add(expected, actual);
                    }
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void LinkedRootTilesConfigurationTest()
        {
            //---------------Set up test pack-------------------
            var allLinkedRootTiles = this.linkedRootTiles.GetRegisteredTypes();
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "LinkedRootTile: {0}";
            var actual = String.Empty;
            var expected = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsAnyLinkedRootTilePredicate>((expectedLinkedRootTileTileConfig) =>
            {
                var expectedLinkedRootTileConfigName = String.Format(linkedRootTileConfSuffix, expectedLinkedRootTileTileConfig.Name);
                var expectedLinkedRootTileType = allLinkedRootTiles.Where(x => x.Name == expectedLinkedRootTileConfigName).FirstOrDefault();
                if (expectedLinkedRootTileType != null)
                {
                    var haloRootTileLinkedConfiguration = expectedLinkedRootTileType.GetInterface(typeof(IHaloRootTileLinkedConfiguration<>).Name);
                    if (haloRootTileLinkedConfiguration == null)
                    {
                        expected = String.Format(messageTemplate, String.Format("{0} to have IHaloRootTileLinkedConfiguration", expectedLinkedRootTileConfigName));
                        actual = String.Format(messageTemplate, String.Format("{0} does not have IHaloRootTileLinkedConfiguration", expectedLinkedRootTileConfigName));
                        configurations.Add(expected, actual);
                    }
                    else
                    {
                        //need to test that that the linked root tile is linked to the correct root tile as per config.json
                        this.haloUIConfigTree.Traverse<IsAnyRootTileWithLinkedRootTilesPredicate>((rootTileConfig) =>
                        {
                            var rootTileConfigName = String.Format(rootTileConfSuffix, rootTileConfig.Name);
                            var isParent = rootTileConfig.LinkedRootTiles.Where(x => x.Id == expectedLinkedRootTileTileConfig.Id).Any();
                            var actualRootTileLinkedTo = haloRootTileLinkedConfiguration.GetGenericArguments().First();
                            var actualRootTileName = actualRootTileLinkedTo.Name;
                            if (isParent && rootTileConfigName != actualRootTileName)
                            {
                                expected = String.Format(messageTemplate, String.Format("{0} to be linked to {1}", expectedLinkedRootTileConfigName, rootTileConfigName));
                                actual = String.Format(messageTemplate, String.Format("{0} is linked to {1}", expectedLinkedRootTileConfigName, actualRootTileName));
                                configurations.Add(expected, actual);
                            }
                        });
                    }
                }
                else
                {
                    expected = String.Format(messageTemplate, String.Format("could not locate LinkedRootTile named: {0}", expectedLinkedRootTileConfigName));
                    actual = String.Format(messageTemplate, String.Format("a LinkedRootTile named: {0}", expectedLinkedRootTileConfigName));
                    configurations.Add(expected, actual);
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

    }
}
