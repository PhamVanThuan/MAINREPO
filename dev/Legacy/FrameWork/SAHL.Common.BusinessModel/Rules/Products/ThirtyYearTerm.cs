using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    [RuleDBTag("AttributeThirtyTermVarifixRule",
       "The client may not have both the 30 year term and and a Varifix product simultaneously.",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Products.AttributeThirtyTermVarifixRule")]
    [RuleInfo]
    public class AttributeThirtyTermVarifixRule : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeThirtyTermVarifixRule rule expects a Domain object to be passed.");

            IAccountVariFixLoan account = Parameters[0] as IAccountVariFixLoan;
            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (account == null && application == null)
                throw new ArgumentException("The AttributeThirtyTermVarifixRule rule expects the following objects to be passed: IAccountVariFixLoan, IApplicationMortgageLoan.");

            #endregion

            if (account != null)
            {
                if (account.SecuredMortgageLoan.InitialInstallments == (30 * 12)) //
                {
                    AddMessage("The client may not have both the 30 year term and and a Varifix product simultaneously.", "", Messages);
                }
            }


            if (application != null)
            {
                IApplicationProductVariFixLoan applicationProductVariFixLoan = (IApplicationProductVariFixLoan)application.CurrentProduct;

                if (applicationProductVariFixLoan.Term == (30 * 12))
                {
                    AddMessage("The client may not have both the 30 year term and and a Varifix product simultaneously.", "", Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("Attribute30Term",
        "The maximum initial term for any loan is 30 years, this means that the period can not ever exceed 30 years",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Products.Attribute30Term")]
    [RuleInfo]
    public class Attribute30Term : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if(Parameters.Length == 0)
                throw new ArgumentException("The Attribute30Term rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount || Parameters[0] is IApplication))
                throw new ArgumentException("The Attribute30Term rule expects the following objects to be passed: IAccount, IApplication.");

            if (Parameters[0] is IAccount)
            {
                // Every product has to have an initial term of 30 years.
                IMortgageLoanAccount mortgageLoanAccount = (Parameters[0] as IMortgageLoanAccount);
                if (mortgageLoanAccount == null)
                    return 1;

                foreach (IFinancialService financialService in mortgageLoanAccount.FinancialServices)
                {
                    IMortgageLoan mortgageLoan = financialService as IMortgageLoan;

                    if (mortgageLoan.InitialInstallments  > (30 * 12))
                    {
                        AddMessage("The maximum initial term for any loan is 30 years.", "", Messages);
                        break;
                    }
                }
            }

            if (Parameters[0] is IApplication)
            {
                IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
                if (applicationMortgageLoan == null)
                    return 1;

                ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;

                if (supportsVariableLoanApplicationInformation != null)
                {
                    if (supportsVariableLoanApplicationInformation.VariableLoanInformation.Term == (30 * 12))
                        AddMessage("The maximum initial term for any loan is 30 years.", "", Messages);
                }
            }

            return 0;
        }
    }
}
