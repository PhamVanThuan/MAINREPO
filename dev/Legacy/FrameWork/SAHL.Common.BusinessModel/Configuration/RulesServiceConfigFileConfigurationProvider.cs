using System;
using SAHL.Common.BusinessModel.Interfaces.Configuration;
using SAHL.Common.Configuration;

namespace SAHL.Common.BusinessModel.Configuration
{
    public class RulesServiceConfigFileConfigurationProvider : ConfigFileConfigurationProviderBase, IRulesServiceConfigurationProvider
    {
        public bool Enabled
        {
            get
            {
                SAHLRulesSection section = base.Config.GetSection("SAHLRules") as SAHLRulesSection;
                if (section == null)
                    throw new Exception("Configuration section SAHLRules does not exist in the application config file.");
                return section.Enabled;
            }
        }
    }
}