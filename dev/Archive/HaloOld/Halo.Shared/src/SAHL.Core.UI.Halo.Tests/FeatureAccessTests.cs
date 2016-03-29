using NUnit.Framework;
using SAHL.Config.Services.Halo.Modules.Client.Tiles;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.Config.Website.Halo.Tests
{
    [TestFixture]
    public class FeatureAccessTests
    {
        [Test, TestCaseSource(typeof(FeatureAccessTests), "GetTileConfigurations")]
        [Category("FeatureAccessTests")]
        public void EnsureTileFeatureAccessIsNamedCorrectly(Type configTypes)
        {
            IRequiredFeatureAccess featureAccess = (IRequiredFeatureAccess)Activator.CreateInstance(configTypes);
            string expected = featureAccess.RequiredFeatureAccess.RemoveFromEnd("Access");
            expected += "Access";
            var actual = featureAccess.RequiredFeatureAccess;
            Assert.AreEqual(expected, actual, string.Format("Feature Access for {0} is currently {1} and is  named incorrectly. It should be ending with Access", configTypes.Name, actual));
            Assert.IsNotEmpty(featureAccess.RequiredFeatureAccess.RemoveFromEnd("Access"), string.Format("Feature Access for {0} is missing prefix. It should be ending with Access", configTypes.Name));
            
        }

        public IEnumerable<Type> GetTileConfigurations()
        {
            return this.GetTileConfigurationsByType(typeof(AbstractTileConfiguration<>));
        }

        private IEnumerable<Type> GetTileConfigurationsByType(Type type)
        {
            Assembly sahl_config_website_halo_assembly = Assembly.GetAssembly(typeof(LegalEntityRootTileConfiguration));
            Type[] allTypes = sahl_config_website_halo_assembly.GetTypes();
            List<Type> configTypes = new List<Type>();
            foreach (var assemblyType in allTypes)
            {
                if (assemblyType.BaseType.IsGenericType)
                {
                    if (assemblyType.BaseType.GetGenericTypeDefinition() == type)
                        configTypes.Add(assemblyType);
                    else
                    {
                        if (IsBaseType(type, assemblyType.BaseType))
                            configTypes.Add(assemblyType);
                    }
                }
            }
            return configTypes;
        }

        private bool IsBaseType(Type type, Type typeToCheck)
        {
            if (typeToCheck.BaseType.IsGenericType)
            {
                if (typeToCheck.BaseType.GetGenericTypeDefinition() == type)
                    return true;
                else
                    return IsBaseType(type, typeToCheck.BaseType);
            }
            else
                return false;
        }
    }

    public static class Extentions
    {
        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }
    }
}