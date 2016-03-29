using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using System;

namespace SAHL.Common.BusinessModel.Rules.Application
{
    [RuleDBTag("ComcorpApplicationRequired",
    "Application must be a Comcorp Application",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.Comcorp.ComcorpApplicationRequired")]
    public class ComcorpApplicationRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication app = Parameters[0] as IApplication;

            if (!app.IsComcorp())
            {
                string err = String.Format("Application: {0} is not a Comcorp application.", app.Key);
                AddMessage(err, err, Messages);
                return 0;
            }
            return 1;
        }
    }
}