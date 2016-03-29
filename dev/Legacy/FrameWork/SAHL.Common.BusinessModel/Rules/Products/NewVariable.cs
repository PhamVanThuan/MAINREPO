using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;


namespace SAHL.Common.BusinessModel.Rules.Products
{
    [RuleDBTag("NewVariableMinimumLoanAmount",
    "Miniumum Loan Amount",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.NewVariableMinimumLoanAmount")]
    [RuleParameterTag(new string[] { "@MinimumLoanAmount,140000,9" })]
    [RuleInfo]
    public class NewVariableMinimumLoanAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into NewVariableMinimumLoanAmount Rule", "", Messages);
                return -1;
            }

            IApplicationProductNewVariableLoan nvl = application.CurrentProduct as IApplicationProductNewVariableLoan;
            if (null == nvl)
            {
                AddMessage("Current Product is not IApplicationProductNewVariableLoan", "", Messages);
                return -1;
            }

            double LoanAmount = Convert.ToDouble(nvl.LoanAgreementAmount);
            foreach (IRuleParameter prm in RuleItem.RuleParameters)
            {
                if (prm.Name == "@MinimumLoanAmount")
                {
                    if (Convert.ToDouble(prm.Value) > LoanAmount)
                    {
                        AddMessage(string.Format("Loan is less than minimum allowed amount."), "", Messages);
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            return -1;
        }
    }

    [RuleDBTag("NewVariableMaximumLoanAmount",
    "Miniumum Loan Amount",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Products.NewVariableMaximumLoanAmount")]
    [RuleParameterTag(new string[] {"@MaximumLoanAmount,5000000,9"})]
    [RuleInfo]
    public class NewVariableMaximumLoanAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into NewVariableMinimumLoanAmount Rule", "", Messages);
                return -1;
            }

            IApplicationProductNewVariableLoan nvl = application.CurrentProduct as IApplicationProductNewVariableLoan;
            if (null == nvl)
            {
                AddMessage("Current Product is not IApplicationProductNewVariableLoan", "", Messages); 
                return -1;
            }

            double LoanAmount = Convert.ToDouble(nvl.LoanAgreementAmount);
            foreach (IRuleParameter prm in RuleItem.RuleParameters)
            {
                if (prm.Name == "@MaximumLoanAmount")
                {
                    if (Convert.ToDouble(prm.Value) < LoanAmount)
                    {
                        AddMessage(string.Format("Loan is more than maximum allowed amount."), "", Messages);
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            return -1;
        }
    }
}
