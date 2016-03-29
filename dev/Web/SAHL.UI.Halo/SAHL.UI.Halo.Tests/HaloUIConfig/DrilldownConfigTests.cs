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
    public class DrilldownConfigTests : HaloUIConfigTests
    {
        [Test]
        public void TileDrilldownConfigurationsTest()
        {
            //---------------Set up test pack-------------------
            var tileOrPageDrilldowns                = this.tileOrPageDrilldowns.GetRegisteredTypes();
            var expectedTileDrilldownConfigurations = new Dictionary<string, string>();
            var messageTemplate                     = "TileDrillDown: {0}";
            var actual                              = String.Empty;
            var expected                            = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse((expectedConfig) =>
            {
                if (expectedConfig.HasChildTileDrilldown)
                {
                    var expectedTileDrilldownConfigName = String.Format(tileOrPageDrilldownConfSuffix, expectedConfig.Name, expectedConfig.Type);
                    var matchingActualTileDrilldowns = tileOrPageDrilldowns.Where(x => expectedTileDrilldownConfigName == x.Name).Select(x => x.Name).ToArray();
                    actual = String.Empty;
                    if (matchingActualTileDrilldowns.Count() == 0)
                    {
                        actual = String.Format(messageTemplate, "MISSING / INCORRECTLY NAMED");
                    }
                    if (matchingActualTileDrilldowns.Count() > 1)
                    {
                        actual = String.Format(messageTemplate, "DUPLICATED");
                    }
                    if (!String.IsNullOrEmpty(actual))
                    {
                        expected = String.Format(messageTemplate, expectedTileDrilldownConfigName);
                        expectedTileDrilldownConfigurations.Add(expected, actual);
                    }
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(expectedTileDrilldownConfigurations);
        }

        [Test]
        public void PageDrilldownConfigurationsTest()
        {
            //---------------Set up test pack-------------------
            var tileOrPageDrilldowns = this.tileOrPageDrilldowns.GetRegisteredTypes();
            var expectedPageDrilldownConfigurations = new Dictionary<string, string>();
            var messageTemplate = "PageDrillDown: {0}";
            var actual = String.Empty;
            var expected = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse((expectedConfig) =>
            {
                if (expectedConfig.HasChildPageDrilldown)
                {
                    var expectedPageDrilldownConfigName = String.Format(tileOrPageDrilldownConfSuffix, expectedConfig.Name, ChildPage);
                    var matchingActualPageDrilldowns = tileOrPageDrilldowns.Where(x => expectedPageDrilldownConfigName == x.Name).Select(x => x.Name).ToArray();
                    actual = String.Empty;
                    if (matchingActualPageDrilldowns.Count() == 0)
                    {
                        actual = String.Format(messageTemplate, "MISSING / INCORRECTLY NAMED");
                    }
                    if (matchingActualPageDrilldowns.Count() > 1)
                    {
                        actual = String.Format(messageTemplate, "DUPLICATED");
                    }
                    if (!String.IsNullOrEmpty(actual))
                    {
                        expected = String.Format(messageTemplate, expectedPageDrilldownConfigName);
                        expectedPageDrilldownConfigurations.Add(expected, actual);
                    }
                }
            });

            //---------------Test Result -----------------------
            AssertConfiguration(expectedPageDrilldownConfigurations);
        }
    }
}
