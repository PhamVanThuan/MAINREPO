using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Base;
using SAHL.Common.Configuration;
using System.Configuration;

namespace SAHL.Common.Service.Mandates
{
    internal class MandatePluginManager : PluginManager
    {
        protected static MandatePluginManager _pluginManager = null;
        /// <summary>
        /// Access method for singleton object.
        /// </summary>
        /// <returns></returns>
        public static MandatePluginManager Access()
        {
            lock (_lockObject)
            {
                if (_pluginManager == null)
                {
                    MandatePluginManager tmp = new MandatePluginManager();
                    if (tmp.Start())
                    {
                        _pluginManager = tmp;
                        tmp = null;
                    }
                    else
                    {
                        throw new Exception("Unable to Start Mandate Manager");
                    }
                }
            }
            return (MandatePluginManager)_pluginManager;
        }

        protected override bool LoadSection()
        {
            // load the details from the config file
            SAHLMandateSection section = ConfigurationManager.GetSection("SAHLMandates") as SAHLMandateSection;
            if (section == null)
                throw new Exception("Configuration section SAHLMandateSection does not exist in the application config file.");
            if (section.Assemblies.Count == 0)
                throw new Exception("There are no assemblies defined in the SAHLMandateSection configuration section.");

            MandateElement ruleElement = section.Assemblies[0];
            _pluginFolder = ruleElement.Location;
            _assemblyName = ruleElement.AssemblyName;
            return true;
        }
    }
}
