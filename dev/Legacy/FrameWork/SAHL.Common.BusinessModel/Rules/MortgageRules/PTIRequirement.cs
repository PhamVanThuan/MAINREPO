using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.MortgageRules
{
    /// <summary>
    /// Checks the PTI Requirement
    /// Params: 
    /// 0: IApplication
    /// </summary>
    [RuleDBTag("MortgageLoanPTIRequirement",
        "LTV must not be less tan 102%",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.MortgageRules.MortgageLoanPTIRequirement")]
    [RuleParameterTag(new string[] {"@PTI,35,9"})]
    [RuleInfo]
    public class MortgageLoanPTIRequirement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into MortgageLoanPTIRequirement Rule", "", Messages);
                return -1;
            }
            IApplicationProductMortgageLoan apml = application.CurrentProduct as IApplicationProductMortgageLoan;
            if (null == apml)
            {
                AddMessage("Application is not a Mortgage Loan", "", Messages);
                return -1;
            }
            ISupportsVariableLoanApplicationInformation vli = apml as ISupportsVariableLoanApplicationInformation;
            if (null != vli)
            {
                foreach (IRuleParameter prm in RuleItem.RuleParameters)
                {
                    if (prm.Name == "@PTI")
                    {
                        int PTI = Convert.ToInt32(prm.Value);
                        if (vli.VariableLoanInformation.PTI > PTI)
                        {
                            AddMessage(string.Format("PTI:({1}) greater than allowed value for application:{0}", application.Key, PTI), "", Messages);
                            return -1;
                        }
                        break;
                    }
                }
            }

            return 1;
        }
    }
}
