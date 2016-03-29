using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.Rules.Service
{
    public class RulesProxyIPlugin : MarshalByRefObject, IBusinessRule
    {
        protected RulesRemotePluginDomainLoader RemotePluginLoader = null;
        IBusinessRule i = null;
        public RulesProxyIPlugin(RulesRemotePluginDomainLoader rl, string RuleName)
        {
            this.RemotePluginLoader = rl;
            i = RemotePluginLoader.GetRemotePlugin(RuleName);
            if (i == null)
                throw new Exception(string.Format("Unable to load rule:{0} from rules.dll", RuleName));
        }

        #region IBusinessRule Members

        public bool StartRule(IDomainMessageCollection Messages, IRuleItem ruleItem)
        {
            return i.StartRule(Messages, ruleItem);
        }

        public int ExecuteRule(IDomainMessageCollection Messages, object[] Parameters)
        {

            return i.ExecuteRule(Messages, Parameters);
        }

        public void CompleteRule()
        {
            i.CompleteRule();
        }

        #endregion
    }
}
