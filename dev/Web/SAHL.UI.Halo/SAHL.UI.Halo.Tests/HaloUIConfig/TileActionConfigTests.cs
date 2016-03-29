using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SAHL.UI.Halo.Tests.HaloUIConfig
{
    [TestFixture]
    public class TileActionConfigTests : HaloUIConfigTests
    {
        [Test]
        public void TileActionConfigurationsTest()
        {
            //---------------Set up test pack-------------------
            var tilesActions                     = this.tileActions.GetRegisteredTypes();
            var expectedTileActionConfigurations = new Dictionary<string, string>();
            var messageTemplate                  = "TileAction: {0}";
            var actual                           = String.Empty;
            var expected                         = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse((expectedConfig) => {
                if (expectedConfig.Type == TileAction)
                {
                    var expectedTileActionConfigName = String.Format(tileActionConfSuffix, expectedConfig.Name);
                    var matchingActualTileActions = tilesActions.Where(x => expectedTileActionConfigName == x.Name).Select(x => x.Name).ToArray();
                    actual = String.Empty;
                    if (matchingActualTileActions.Count() == 0)
                    {
                        actual = String.Format(messageTemplate, "MISSING / INCORRECTLY NAMED");
                    }
                    if (matchingActualTileActions.Count() > 1)
                    {
                        actual = String.Format(messageTemplate, "DUPLICATED");
                    }
                    if (!String.IsNullOrEmpty(actual))
                    {
                        expected = String.Format(messageTemplate, expectedTileActionConfigName);
                        if (!expectedTileActionConfigurations.ContainsKey(expected))
                        {
                            expectedTileActionConfigurations.Add(expected, actual);
                        }
                    }
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(expectedTileActionConfigurations);
        }
    }
}
