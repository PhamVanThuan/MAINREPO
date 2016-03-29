using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Application.Reasons
{
    [RuleDBTag("ApplicationReasonTypeAdd",
    "NTU or Decline reasons can be added for (1) Loan applicaiotns or (2) Quickcash",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.Reasons.ApplicationReasonTypeAdd")]
    [RuleInfo]
    public class ApplicationReasonTypeAdd : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationReasonTypeAdd rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationReasonTypeAdd rule expects the following objects to be passed: IApplication.");

            //if (RuleItem.RuleParameters.Count < 1)
            //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            # region Rule Check

            IApplication application = Parameters[0] as IApplication;

            switch ((OfferTypes)application.ApplicationType.Key)
            {
                case OfferTypes.ReAdvance:
                case OfferTypes.FurtherAdvance:
                case OfferTypes.FurtherLoan:
                case OfferTypes.SwitchLoan:
                case OfferTypes.NewPurchaseLoan:
                case OfferTypes.RefinanceLoan:
                    return 0;
                default: 
                    AddMessage(String.Format("NTU or Decline reasons not of type 'Loan Application' or 'Quick Cash'"), "", Messages);
                    break;
            }

            #endregion

            return 0;
         }
    }


    [RuleDBTag("ApplicationReasonUpdate",
    "Users should only be able to add and remove reasons to open applications",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.Reasons.ApplicationReasonUpdate")]
    [RuleInfo]
    public class ApplicationReasonUpdate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationReasonUpdate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationReasonUpdate rule expects the following objects to be passed: IApplication.");

            //if (RuleItem.RuleParameters.Count < 1)
            //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            # region Rule Check


            IApplication application = Parameters[0] as IApplication;

            if (application.ApplicationStatus.Key == (int)OfferStatuses.Open)
                return 0;
            else
                AddMessage(String.Format("Reasons can only be added to an open application."), "", Messages);

            #endregion

            return 0;

        }
    }


    [RuleDBTag("ApplicationReasonMiscellaneousEnforceComment",
    "If Miscellaneous Reason is chosen then a comment is enforced.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.Reasons.ApplicationReasonMiscellaneousEnforceComment")]
    [RuleInfo]
    public class ApplicationReasonMiscellaneousEnforceComment : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationReasonMiscellaneousEnforceComment rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IReason))
                throw new ArgumentException("The ApplicationReasonMiscellaneousEnforceComment rule expects the following objects to be passed: IReason.");

            //if (RuleItem.RuleParameters.Count < 1)
            //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            # region Rule Check


            IReason reason = Parameters[0] as IReason;

            if (reason.ReasonDefinition.ReasonDescription.Key == (int)ReasonDescriptions.MiscellaneousReason) 
            {
                if (string.IsNullOrEmpty(reason.Comment) || reason.Comment.Trim().Length == 0)
                {
                    string msg = "Miscellaneous Reason chosen - Please enter a comment.";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }

            #endregion

            return 1;

        }
    }


}
