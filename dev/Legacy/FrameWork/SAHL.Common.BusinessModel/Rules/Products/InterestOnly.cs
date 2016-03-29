using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

//using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    [RuleDBTag("AttributeInterestOnlyVarifix",
        "AttributeInterestOnlyVarifix",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyVarifix")]
    [RuleInfo]
    public class AttributeInterestOnlyVarifix : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyVarifix rule expects a Domain object to be passed.");

            IApplicationMortgageLoan mortgageLoan = Parameters[0] as IApplicationMortgageLoan;
            if (mortgageLoan == null)
                throw new ArgumentException("The AttributeInterestOnlyVarifix rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion Check for allowed object type(s)

            IApplicationProductVariFixLoan applicationProductVariFixLoan = (IApplicationProductVariFixLoan)mortgageLoan.CurrentProduct;

            // Look for the Interest-Only rate override ...
            bool interestOnlySelected = false;
            foreach (IApplicationInformationFinancialAdjustment applicationInformationFA in applicationProductVariFixLoan.VariFixInformation.ApplicationInformation.ApplicationInformationFinancialAdjustments)
            {
                if (applicationInformationFA.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                {
                    interestOnlySelected = true;
                    break;
                }
            }

            if (interestOnlySelected)
            {
                string errorMessage = "A client that selected Varifix may not have the Interest Only product.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyMinimumLoanAmount",
        "AttributeInterestOnlyMinimumLoanAmount",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyMinimumLoanAmount")]
    [RuleParameterTag(new string[] { "@MinLoanAmount,250000,9" })]
    [RuleInfo]
    public class AttributeInterestOnlyMinimumLoanAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyMinimumLoanAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The AttributeInterestOnlyMinimumLoanAmount rule expects the following objects to be passed: IApplication.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null)
                return 1;

            double minLoanAgreementAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            bool isLessThanMinimumLoan = false;

            // Get the LoanAgreementAmount
            IApplicationProductMortgageLoan applicationProductMortgageLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductMortgageLoan;

            if (applicationProductMortgageLoan.LoanAgreementAmount < minLoanAgreementAmount)
            {
                // If this is an Interest-Only Loan - Scream
                IApplicationInformation applicationInformation = applicationMortgageLoan.GetLatestApplicationInformation();

                if (applicationMortgageLoan != null)
                {
                    foreach (IApplicationInformationFinancialAdjustment applicationInformationFA in applicationInformation.ApplicationInformationFinancialAdjustments)
                    {
                        if (applicationInformationFA.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                        {
                            isLessThanMinimumLoan = true;
                            break;
                        }
                    }
                }
            }

            if (isLessThanMinimumLoan)
            {
                string errorMessage = String.Format("The Minimum Loan Amount is {0}.", minLoanAgreementAmount);
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyMaximumLoanAmount",
        "AttributeInterestOnlyMaximumLoanAmount",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyMaximumLoanAmount")]
    [RuleParameterTag(new string[] { "@maxLoanAmount,3000000,9" })]
    [RuleInfo]
    public class AttributeInterestOnlyMaximumLoanAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyMaximumLoanAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The AttributeInterestOnlyMaximumLoanAmount rule expects the following objects to be passed: IApplication.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null)
                return 1;

            double maxLoanAgreementAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            bool isGreaterThanMaximumLoan = false;

            // Get the LoanAgreementAmount
            IApplicationProductMortgageLoan applicationProductMortgageLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductMortgageLoan;

            if (applicationProductMortgageLoan.LoanAgreementAmount > maxLoanAgreementAmount)
            {
                // If this is an Interest-Only Loan - Scream
                IApplicationInformation applicationInformation = applicationMortgageLoan.GetLatestApplicationInformation();

                if (applicationMortgageLoan != null)
                {
                    foreach (IApplicationInformationFinancialAdjustment applicationInformationFA in applicationInformation.ApplicationInformationFinancialAdjustments)
                    {
                        if (applicationInformationFA.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                        {
                            isGreaterThanMaximumLoan = true;
                            break;
                        }
                    }
                }
            }

            if (isGreaterThanMaximumLoan)
            {
                string errorMessage = String.Format("The maximum Loan Amount is {0}.", maxLoanAgreementAmount);
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyMaxLTV",
        "AttributeInterestOnlyMaxLTV",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyMaxLTV")]
    [RuleParameterTag(new string[] { "@MaxLTV,90,9" })]
    [RuleInfo]
    public class AttributeInterestOnlyMaxLTV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyMaxLTV rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The AttributeInterestOnlyMaxLTV rule expects the following objects to be passed: IApplication.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null)
                return 1;

            IApplicationInformation applicationInformation = applicationMortgageLoan.GetLatestApplicationInformation();

            // Is this an interest-only loan
            bool isInterestOnly = false;
            foreach (IApplicationInformationFinancialAdjustment applicationInformationFA in applicationInformation.ApplicationInformationFinancialAdjustments)
            {
                if (applicationInformationFA.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                {
                    isInterestOnly = true;
                    break;
                }
            }

            // Get the LoanAgreementAmount
            double maxPTI = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
            {
                if (isInterestOnly
                    && supportsVariableLoanApplicationInformation.VariableLoanInformation.LTV > maxPTI)
                {
                    string errorMessage = String.Format("Loans with an LTV of greater than {0}% may not qualify for Interest Only.", maxPTI);
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyMaxLoanAmountLTV80",
    "AttributeInterestOnlyMaxLoanAmountLTV80",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyMaxLoanAmountLTV80")]
    [RuleParameterTag(new string[] { "@MaxPTI80,80,9", "@MinLoan80,1500000,9", "@MaxLoan80,3000000,9" })]
    [RuleInfo]
    public class AttributeInterestOnlyMaxLoanAmountLTV80 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyMaxLoanAmountLTV80 rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The AttributeInterestOnlyMaxLoanAmountLTV80 rule expects the following objects to be passed: IApplication.");

            if (RuleItem.RuleParameters.Count < 3)
                throw new Exception(String.Format("The rule {0} expects three parameters to be configured.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            double maxPTI = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
            double minLoan = Convert.ToDouble(RuleItem.RuleParameters[1].Value);
            double maxLoan = Convert.ToDouble(RuleItem.RuleParameters[2].Value);

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null)
                return 1;

            IApplicationInformation applicationInformation = applicationMortgageLoan.GetLatestApplicationInformation();

            // Is this an interest-only loan
            bool isInterestOnly = false;
            foreach (IApplicationInformationFinancialAdjustment applicationInformationFA in applicationInformation.ApplicationInformationFinancialAdjustments)
            {
                if (applicationInformationFA.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                {
                    isInterestOnly = true;
                    break;
                }
            }

            if (isInterestOnly)
            {
                // Get the LoanAgreementAmount
                ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (supportsVariableLoanApplicationInformation != null)
                {
                    // Does the Loan Amount fall within the applicable range?
                    if (supportsVariableLoanApplicationInformation.VariableLoanInformation.LoanAgreementAmount > minLoan
                        && supportsVariableLoanApplicationInformation.VariableLoanInformation.LoanAgreementAmount <= maxLoan
                        && supportsVariableLoanApplicationInformation.VariableLoanInformation.LTV > maxPTI)
                    {
                        string errorMessage = String.Format("Loans within the range: {0} - {1}, with an LTV of greater than {0}% may not qualify for Interest Only.", minLoan, maxLoan, maxPTI);
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyMaxLoanAmountLTV90",
    "AttributeInterestOnlyMaxLoanAmountLTV90",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyMaxLoanAmountLTV90")]
    [RuleParameterTag(new string[] { "@MaxPTI90,90,9", "@MinLoan90,250000,9", "@MaxLoan90,1500000,9" })]
    [RuleInfo]
    public class AttributeInterestOnlyMaxLoanAmountLTV90 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyMaxLoanAmountLTV80 rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The AttributeInterestOnlyMaxLoanAmountLTV80 rule expects the following objects to be passed: IApplication.");

            if (RuleItem.RuleParameters.Count < 3)
                throw new Exception(String.Format("The rule {0} expects three parameters to be configured.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            double maxPTI = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
            double minLoan = Convert.ToDouble(RuleItem.RuleParameters[1].Value);
            double maxLoan = Convert.ToDouble(RuleItem.RuleParameters[2].Value);

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null)
                return 1;

            IApplicationInformation applicationInformation = applicationMortgageLoan.GetLatestApplicationInformation();

            // Is this an interest-only loan
            bool isInterestOnly = false;
            foreach (IApplicationInformationFinancialAdjustment applicationInformationFA in applicationInformation.ApplicationInformationFinancialAdjustments)
            {
                if (applicationInformationFA.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                {
                    isInterestOnly = true;
                    break;
                }
            }

            if (isInterestOnly)
            {
                // Get the LoanAgreementAmount
                ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (supportsVariableLoanApplicationInformation != null)
                {
                    // Does the Loan Amount fall within the applicable range?
                    if (supportsVariableLoanApplicationInformation.VariableLoanInformation.LoanAgreementAmount > minLoan
                        && supportsVariableLoanApplicationInformation.VariableLoanInformation.LoanAgreementAmount <= maxLoan)
                    {
                        if (supportsVariableLoanApplicationInformation.VariableLoanInformation.LTV > maxPTI)
                        {
                            string errorMessage = String.Format("Loans within the range: {0} - {1}, with an LTV of greater than {0}% may not qualify for Interest Only.", minLoan, maxLoan, maxPTI);
                            AddMessage(errorMessage, errorMessage, Messages);
                        }
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyArrears",
        "AttributeInterestOnlyArrears",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyArrears")]
    [RuleParameterTag(new string[] { "@MaxMonthsInArrears,2.0,9" })]
    [RuleInfo]
    public class AttributeInterestOnlyArrears : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyArrears rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The AttributeInterestOnlyArrears rule expects the following objects to be passed: IAccount.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

            if (mortgageLoanAccount == null)
                return 1;

            double maxMonthsInArrears = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (mortgageLoanAccount.InstallmentSummary.MonthsInArrears >= maxMonthsInArrears)
            {
                string errorMessage = String.Format("A client may not opt into Interest Only if their Account is more than {0} months in arrears.", maxMonthsInArrears);
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyRateReset",
        "AttributeInterestOnlyRateReset",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyRateReset")]
    [RuleInfo]
    public class AttributeInterestOnlyRateReset : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyRateReset rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The AttributeInterestOnlyRateReset rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

            if (mortgageLoanAccount == null)
                return 1;

            bool foundNonEighteenth = false;
            foreach (IFinancialService financialService in mortgageLoanAccount.FinancialServices)
            {
                IMortgageLoan mortgageLoan = financialService as IMortgageLoan;
                if (mortgageLoan != null)
                {
                    if (mortgageLoan.ResetConfiguration.Key != (int)ResetConfigurations.Eighteenth)
                    {
                        foundNonEighteenth = true;
                        break;
                    }
                }
            }

            if (foundNonEighteenth)
            {
                string errorMessage = "Interest Only is only available to 18 reset clients. This client will need to have the reset date changed before Opt In.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("AttributeInterestOnlyUnderCancellation",
        "AttributeInterestOnlyUnderCancellation",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AttributeInterestOnlyUnderCancellation")]
    [RuleInfo]
    public class AttributeInterestOnlyUnderCancellation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AttributeInterestOnlyUnderCancellation rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The AttributeInterestOnlyUnderCancellation rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

            if (mortgageLoanAccount == null)
                return 1;

            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            IReadOnlyEventList<IDetail> details = accountRepository.GetDetailByAccountKeyAndDetailType(mortgageLoanAccount.Key, (int)DetailTypes.UnderCancellation);

            bool underCancellation = false;
            foreach (IDetail detail in details)
            {
                if (detail.DetailType.Key == (int)DetailTypes.UnderCancellation)
                {
                    underCancellation = true;
                    break;
                }
            }

            if (underCancellation)
            {
                string errorMessage = "The Account is Under Debt Councelling and therefore may not opt into Interest Only.";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    /// <summary>
    /// To be run as part of the opt in procedure.
    /// </summary>
    [RuleDBTag("ProductInterestOnlyOptInLoanTransaction",
   "ProductInterestOnlyOptInLoanTransaction",
      "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Products.ProductInterestOnlyOptInLoanTransaction")]
    [RuleInfo]
    public class ProductInterestOnlyOptInLoanTransaction : BusinessRuleBase
    {
        public ProductInterestOnlyOptInLoanTransaction(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductInterestOnlyOptInLoanTransaction rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplication))
                throw new ArgumentException("The ProductInterestOnlyOptInLoanTransaction rule expects one of the following objects to be passed: IAccount or IApplication.");

            int accountKey = 0;

            if (Parameters[0] is IAccount)
            {
                IAccount account = Parameters[0] as IAccount;
                accountKey = account.Key;
            }

            if (Parameters[0] is IApplication)
            {
                IApplication app = Parameters[0] as IApplication;
                if (app.Account != null)
                    accountKey = app.Account.Key;
            }

            #endregion Check for allowed object type(s)

            string sqlQuery = UIStatementRepository.GetStatement("Rules.Products", "ProductInterestOnlyOptInLoanTransaction");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (Convert.ToInt16(o) == 1)
            {
                string errorMessage = "Further lending can not be started because the client has initiated an opt in into InterestOnly. The opt in must be completed or cancelled before you can continue.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }
}