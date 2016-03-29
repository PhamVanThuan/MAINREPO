using NUnit.Framework;
using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Configuration;
using SAHL.Config.Services.Halo.Modules.Client.Tiles;

namespace SAHL.Config.Services.Halo.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test, TestCaseSource(typeof(ConfigurationTests), "GetActionConfigurations")]
        public void EnsureActionTileConfigurationsAreNamedCorrectly(Type actionTypes)
        {
            string expectedName = "";
            
            //Get the name of the tile that we are using.
            Type modelType = actionTypes.BaseType.GetGenericArguments()[0];
            expectedName = modelType.Name.Replace("TileModel","");

            expectedName += "Configuration";
            Assert.AreEqual(expectedName, actionTypes.Name, String.Format("{0} is named incorrectly for action {1}. It should be {2}", actionTypes.Name, modelType.Name, expectedName));
        }

        [Test, TestCaseSource(typeof(ConfigurationTests), "GetMajorTileConfigurations")]
        public void EnsureMajorTileConfigurationHasRootOrDrillDown(Type configTypes)
        {
            Type genericRootType = (typeof(IRootTileConfiguration<>));
            Type genericDrillDownType = (typeof(IDrillDownTileConfiguration<>));
            var genericTypes = configTypes.GetInterfaces().Where(x => x.IsGenericType);
            Assert.That(genericTypes.Any(x => x.GetGenericTypeDefinition() == genericRootType || x.GetGenericTypeDefinition() == genericDrillDownType), "{0} does not have a root or drilldown configured for it.", configTypes);
        }

        [Test, TestCaseSource(typeof(ConfigurationTests), "GetMajorTileConfigurations")]
        public void EnsureMajorTileConfigurationIsNamedCorrectly(Type configTypes)
        {
            string expectedName = "";
            string tileConfigType = "";
            //Get the name of the tile we are using.
            Type modelType = configTypes.BaseType.GetGenericArguments()[0];
            expectedName = modelType.Name.Replace("MajorTileModel", "");

            //Get the configuration interface we are implementing.
            var genericTypes = configTypes.GetInterfaces().Where(x => x.IsGenericType).Select(x=>x.GetGenericTypeDefinition());
            if (genericTypes.Contains(typeof(IRootTileConfiguration<>)))
                tileConfigType = "Root";
            else if (genericTypes.Contains(typeof(IDrillDownTileConfiguration<>)))
                tileConfigType = "DrillDown";
            
            expectedName += tileConfigType;
            expectedName += "TileConfiguration";

            Assert.AreEqual(expectedName, configTypes.Name, String.Format("{0} is incorrectly named for a {1} tile of {2}. It should be {3}.", configTypes.Name, tileConfigType, modelType.Name, expectedName));
        }

        public IEnumerable<Type> GetMajorTileConfigurations()
        {
            return this.GetTileConfigurationsByType(typeof(MajorTileConfiguration<>));
        }

        public IEnumerable<Type> GetActionConfigurations()
        {
            return this.GetTileConfigurationsByType(typeof(ActionTileConfiguration<>));
        }

        private IEnumerable<Type> GetTileConfigurationsByType(Type type)
        {
            Assembly sahl_config_website_halo_assembly = Assembly.GetAssembly(typeof(LegalEntityRootTileConfiguration));
            Type[] allTypes = sahl_config_website_halo_assembly.GetTypes();
            List<Type> configTypes = new List<Type>();
            foreach (var assemblyType in sahl_config_website_halo_assembly.GetTypes())
            {
                if (assemblyType.BaseType.IsGenericType)
                {
                    if (assemblyType.BaseType.GetGenericTypeDefinition() == type)
                        configTypes.Add(assemblyType);
                }
            }
            return configTypes;
        }

    }
}
