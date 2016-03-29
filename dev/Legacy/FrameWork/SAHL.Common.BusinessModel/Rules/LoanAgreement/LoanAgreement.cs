using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.LoanAgreement
{
    [RuleDBTag("MortgageLoanAgreementAmount",
    "Loan Agreement Amount must be between R 0.01 and R 999,999,999.99.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LoanAgreement.MortgageLoanAgreementAmount")]
    [RuleParameterTag(new string[] { "@MinAmount, 0.01, 10", "@MaxAmount, 999999999.99, 10" })]
    [RuleInfo]
    public class MortgageLoanAgreementAmount : BusinessRuleBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is ILoanAgreement))
                throw new ArgumentException("Parameter[0] is not of type ILoanAgreement.");

            ILoanAgreement la = (ILoanAgreement)Parameters[0];

            double min = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
            double max = Convert.ToDouble(RuleItem.RuleParameters[1].Value);

            if (max > la.Bond.BondRegistrationAmount)
                max = la.Bond.BondRegistrationAmount;

            if (la.Amount < min || la.Amount > max)
                AddMessage("Loan Agreement Amount must be between R 0.01 and " + max.ToString(SAHL.Common.Constants.CurrencyFormat) + ".", "Loan Agreement Amount must be between R 0.01 and " + max.ToString(SAHL.Common.Constants.CurrencyFormat) + ".", Messages);


            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAgreementSum",
    "The Total Loan Agreement cannot be less than the loan current balance.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LoanAgreement.MortgageLoanAgreementSum")]
    [RuleInfo]
    public class MortgageLoanAgreementSum : BusinessRuleBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is ILoanAgreement))
                throw new ArgumentException("Parameter[0] is not of type ILoanAgreement.");

            ILoanAgreement la = (ILoanAgreement)Parameters[0];

            IAccount acc = la.Bond.MortgageLoans[0].Account;
            IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
            if (mla == null)
                throw new Exception("Not a Mortgage Loan Account!");

            double laTotal = 0;

            // Check all bonds against variable leg
            foreach (IBond bondV in mla.SecuredMortgageLoan.Bonds)
            {
                if (la.Bond.Key != bondV.Key)
                    laTotal += bondV.BondLoanAgreementAmount;
            }

            // Check all bonds against fixed leg
            IAccountVariFixLoan facc = acc as IAccountVariFixLoan;
            if (facc != null)
            {
                foreach (IBond bondF in facc.FixedSecuredMortgageLoan.Bonds)
                {
                    if (la.Bond.Key != bondF.Key)
                        laTotal += bondF.BondLoanAgreementAmount;
                }
            }
            laTotal += la.Amount;
            string message = string.Format("The Total Loan Agreement cannot be less than the loan current balance - R {0}.",Math.Round(mla.LoanCurrentBalance,2));

            // #Trac 12793 Added the rounding to match the Application & real-world payments of rands and cents
            if (Math.Round(laTotal,2)  < Math.Round(mla.LoanCurrentBalance, 2))
                AddMessage(message, message, Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAgreementBondValue",
   "The Total Loan Agreement Amount cannot be greater than the Bond Amount.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.LoanAgreement.MortgageLoanAgreementBondValue")]
    [RuleInfo]
    public class MortgageLoanAgreementBondValue : BusinessRuleBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is ILoanAgreement))
                throw new ArgumentException("Parameter[0] is not of type ILoanAgreement.");

            ILoanAgreement la = (ILoanAgreement)Parameters[0];

            IBond bond = la.Bond;

            if (bond.BondRegistrationAmount < la.Amount)
                AddMessage("The Loan Agreement Amount cannot be greater than the Bond Amount.", "The Loan Agreement Amount cannot be greater than the Bond Amount.", Messages);
            
            return 0;
        }
    }








}
