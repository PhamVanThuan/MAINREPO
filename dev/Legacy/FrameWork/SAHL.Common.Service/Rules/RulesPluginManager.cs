using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Base;
using SAHL.Common.Configuration;
using System.Configuration;

namespace SAHL.Common.Service.Rules
{
    internal class RulesPluginManager : PluginManager
    {
        protected static RulesPluginManager _pluginManager = null;
        /// <summary>
        /// Access method for singleton object.
        /// </summary>
        /// <returns></returns>
        public static RulesPluginManager Access()
        {
            lock (_lockObject)
            {
                if (_pluginManager == null)
                {
                    RulesPluginManager tmp = new RulesPluginManager();
                    if (tmp.Start())
                    {
                        _pluginManager = tmp;
                        tmp = null;
                    }
                    else
                    {
                        throw new Exception("Unable to Start Plugin Manager");
                    }
                }
            }
            return (RulesPluginManager)_pluginManager;
        }

        protected override bool LoadSection()
        {
            // load the details from the config file
            SAHLRulesSection section = ConfigurationManager.GetSection("SAHLRules") as SAHLRulesSection;
            if (section == null)
                throw new Exception("Configuration section SAHLRules does not exist in the application config file.");
            if (section.Assemblies.Count == 0)
                throw new Exception("There are no assemblies defined in the SAHLRules configuration section.");

            RuleElement ruleElement = section.Assemblies[0];
            _pluginFolder = ruleElement.Location;
            _assemblyName = ruleElement.AssemblyName;
            return true;
        }
    }
}
