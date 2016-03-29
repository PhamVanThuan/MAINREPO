using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Base;
using SAHL.Common.Rules.Service;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Service.Rules
{
    internal class RulesSameDomainLoader : SameDomainLoader
    {
        protected static RulesRemotePluginDomainLoader loader;
        protected static RulesPluginManager pluginManager;

        static RulesSameDomainLoader()
        {
            pluginManager = RulesPluginManager.Access();
            if (RulesSameDomainLoader.loader == null)
            {
                loader = new RulesRemotePluginDomainLoader();
                loader.LoadAssemblies(pluginManager);
            }
        }
        protected override bool CreateRemoteLoader()
        {
            try
            {
                if (loader.LoadAssemblies(pluginManager))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create Plugin Domain.", ex);
            }
        }
        public static IBusinessRule GetPlugin(string RuleName)
        {
            try
            {
                RulesRemotePluginDomainLoader rl = (RulesRemotePluginDomainLoader)loader;
                IBusinessRule i = rl.GetIPluginProxy(RuleName);
                return i;
            }
            catch (Exception ex)
            {
                throw new RuleException(String.Format("Unable to get plugin {0}", RuleName), ex);
            }
        }
    }
}
