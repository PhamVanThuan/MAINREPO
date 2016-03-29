using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.AccountBaselII
{
    [RuleDBTag("AccountBaselIIDecline",
    "There is a very high risk/realistic potential of account going into default/arrears.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.AccountBaselII.AccountBaselIIDecline")]
    [RuleParameterTag(new string[] { "@MinScore,671,9" })]
    [RuleInfo]
    public class AccountBaselIIDecline : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountBaselIIDecline rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;
            IAccount account = Parameters[0] as IAccount;

            if (application == null && account == null)
                throw new ArgumentException("The AccountBaselIIDecline rule expects the following objects to be passed: IApplication or IAccount.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            if (application != null && (application.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
            {
                account = application.Account;
            }
            else if (account == null)
                return 0;

            #endregion

            int minScore = Convert.ToInt32(RuleItem.RuleParameters[0].Value);
            IAccountBaselII accountbasel = account.GetLatestBehaviouralScore();

            if (accountbasel != null && accountbasel.BehaviouralScore <= minScore)
            {
                string msg = string.Format(@"Refer to our guidance on acceptable Behavioural score (Basel II). 
                A score less than or equal to {0} represents a higher risk and typically a decline for further credit.", minScore);
                AddMessage(msg, msg, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("AccountBaselIIRefer",
    "There is moderate risk/potential for the account going into default/arrears.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.AccountBaselII.AccountBaselIIRefer")]
    [RuleParameterTag(new string[] { "@MaxScore,695,9" ,"@MinScore,672,9" })]
    [RuleInfo]
    public class AccountBaselIIRefer : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountBaselIIRefer rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;
            IAccount account = Parameters[0] as IAccount;

            if (application == null && account == null)
                throw new ArgumentException("The AccountBaselIIRefer rule expects the following objects to be passed: IApplication or IAccount.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            if (application != null && (application.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
            {
                account = application.Account;
            }
            else if (account == null)
                return 0;

            #endregion

            int minScore = Convert.ToInt32(RuleItem.RuleParameters[0].Value);
            int maxScore = Convert.ToInt32(RuleItem.RuleParameters[1].Value);

            IAccountBaselII accountbasel = account.GetLatestBehaviouralScore();

            if (accountbasel != null && (accountbasel.BehaviouralScore <= maxScore && accountbasel.BehaviouralScore >= minScore))
            {
                string msg = string.Format(@"Refer to our guidance on acceptable Behavioural score (Basel II). 
                A score between {0} and {1} inclusive suggests some risk & a cautious approach.", minScore,maxScore);
                AddMessage(msg, msg, Messages);
            }

            return 0;
        }
    }
}
