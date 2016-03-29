using System;

//using System.Linq;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    [RuleDBTag("ProductSuperLoMinimum",
        "ProductSuperLoMinimum",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductSuperLoMinimum")]
    [RuleParameterTag(new string[] { "@MinLoanAmount,250000,9" })]
    [RuleInfo]
    public class ProductSuperLoMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductSuperLoMinimum rule expects a Domain object to be passed.");

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (application == null)
                throw new ArgumentException("The ProductSuperLoMinimum rule expects the following objects to be passed: IApplicationMortgageLoan.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = (IApplicationProductSuperLoLoan)application.CurrentProduct;
            double minLoanAgreementAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (applicationProductSuperLoLoan.LoanAgreementAmount < minLoanAgreementAmount)
            {
                string errorMessage = String.Format("The Minimum Loan Amount is {0}.", minLoanAgreementAmount);
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ProductSuperLoNewCat1",
        "ProductSuperLoNewCat1",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductSuperLoNewCat1")]
    [RuleInfo]
    public class ProductSuperLoNewCat1 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductSuperLoNewCat1 rule expects a Domain object to be passed.");

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (application == null)
                throw new ArgumentException("The ProductSuperLoNewCat1 rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion Check for allowed object type(s)

            if (application.CurrentProduct is IApplicationProductSuperLoLoan)
            {
                IApplicationProductSuperLoLoan applicationProductSuperLoLoan = (IApplicationProductSuperLoLoan)application.CurrentProduct;

                if (applicationProductSuperLoLoan.VariableLoanInformation.Category != null)
                {
                    if (applicationProductSuperLoLoan.VariableLoanInformation.Category.Key > (int)Categories.Category1)
                    {
                        string errorMessage = "Any new business with Super Lo product can not originate in Cat 2 or above.";
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ProductSuperLoNewSPV",
        "ProductSuperLoNewSPV",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductSuperLoNewSPV")]
    [RuleInfo]
    public class ProductSuperLoNewSPV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductSuperLoNewSPV rule expects a Domain object to be passed.");

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (application == null)
                throw new ArgumentException("The ProductSuperLoNewSPV rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion Check for allowed object type(s)

            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = (IApplicationProductSuperLoLoan)application.CurrentProduct;

            if (applicationProductSuperLoLoan.VariableLoanInformation.SPV.Key != (int)SPVs.MainStreet65PtyLtd)
            {
                string errorMessage = "Any new business with Super Lo product can only disburse or originate into SPV Main Street 65.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    /// <summary>
    /// To be run as part of the opt in procedure.
    /// </summary>
    [RuleDBTag("ProductSuperLoResetDate",
        "ProductSuperLoResetDate",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductSuperLoResetDate")]
    [RuleInfo]
    public class ProductSuperLoResetDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductSuperLoResetDate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ProductSuperLoResetDate rule expects the following objects to be passed: IAccount.");

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
                string errorMessage = "Super Lo is only available to 18 reset clients. This client will need to have the reset date changed before Opt In.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    /// <summary>
    /// To be run as part of the opt in procedure.
    /// </summary>
    [RuleDBTag("ProductSuperLoOptInLoanTransaction",
      "ProductSuperLoOptInLoanTransaction",
      "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Products.ProductSuperLoOptInLoanTransaction")]
    [RuleInfo]
    public class ProductSuperLoOptInLoanTransaction : BusinessRuleBase
    {
        public ProductSuperLoOptInLoanTransaction(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductSuperLoOptInLoanTransaction rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplication))
                throw new ArgumentException("The ProductSuperLoOptInLoanTransaction rule expects one of the following objects to be passed: IAccount or IApplication.");

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

            string sqlQuery = UIStatementRepository.GetStatement("Rules.Products", "ProductSuperLoOptInLoanTransaction");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (Convert.ToInt16(o) == 1)
            {
                string errorMessage = "Further lending can not be started because the client has initiated an opt in into SuperLo. The opt in must be completed or cancelled before you can continue.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }

    /// <summary>
    /// To be run as part of the opt in procedure.
    /// </summary>
    [RuleDBTag("ProductSuperLoFLSPVChange",
     "ProductSuperLoFLSPVChange",
      "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.Products.ProductSuperLoFLSPVChange")]
    [RuleInfo]
    public class ProductSuperLoFLSPVChange : BusinessRuleBase
    {
        public ProductSuperLoFLSPVChange(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductSuperLoFLSPVChange rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ProductSuperLoFLSPVChange rule expects one of the following objects to be passed: IApplication.");

            IApplication app = Parameters[0] as IApplication;

            #endregion Check for allowed object type(s)

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ProductSuperLoFLSPVChange");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@appKey", app.Key));

            object obj = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (obj != null)
            {
                string errMsg = (string)obj;
                AddMessage(errMsg, errMsg, Messages);
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("SuperLoOptOutRequired",
    "Check if account is an active SuperLo account & LTV or the FL application LTV is above 85%",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.SuperLoOptOutRequired")]
    [RuleParameterTag(new string[] { "@MaxLTV,85,9" })]
    [RuleInfo()]
    public class SuperLoOptOutRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into SuperLoOptOutRequired Rule", "", Messages);
                return -1;
            }

            IApplicationProduct appProd = application.CurrentProduct;
            ISupportsVariableLoanApplicationInformation vli = appProd as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan aivl = vli.VariableLoanInformation;

            if ((int)appProd.ProductType == (int)SAHL.Common.Globals.Products.SuperLo && null != aivl)
            {
                IMortgageLoanAccount mla = application.Account as IMortgageLoanAccount;
                IMortgageLoan vML = mla.SecuredMortgageLoan;
                double accVal = vML.GetActiveValuationAmount();

                double appAmount = aivl.LoanAmountNoFees.HasValue ? aivl.LoanAmountNoFees.Value : 0;
                double initiationFee = 0;
                double registrationFee = 0;

                foreach (IApplicationExpense ae in application.ApplicationExpenses)
                {
                    if (ae.ExpenseType.Key == (int)ExpenseTypes.RegistrationFee)
                        registrationFee = ae.TotalOutstandingAmount;

                    if (ae.ExpenseType.Key == (int)ExpenseTypes.InitiationFeeBondPreparationFee)
                        initiationFee = ae.TotalOutstandingAmount;
                }

                double totalFees = initiationFee + registrationFee;
                double appCB = vML.CurrentBalance + appAmount + totalFees; // vML.CurrentBalance -> vML.CurrentBalance
                double calcLTV = (appCB / accVal) * 100D;

                foreach (IRuleParameter prm in RuleItem.RuleParameters)
                {
                    if (prm.Name == "@MaxLTV")
                    {
                        int LTV = Convert.ToInt32(prm.Value);
                        if (calcLTV > LTV)
                        {
                            AddMessage("The effective LTV of this case is greater than 85%. Please opt this client out of Super Lo.", "The effective LTV of this case is greater than 85%. Please opt this client out of Super Lo.", Messages);
                            return 0;
                        }
                        break;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("SuperLoOptOutCheck",
    "Check if account is an active SuperLo account",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.SuperLoOptOutCheck")]
    [RuleInfo()]
    public class SuperLoOptOutCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into SuperLoOptOutRequired Rule", "", Messages);
                return -1;
            }

            IApplicationProduct appProd = application.CurrentProduct;

            if ((int)appProd.ProductType != (int)SAHL.Common.Globals.Products.SuperLo)
            {
                string msg = string.Format("The current product on the account must be Super Lo to perform this action.");
                AddMessage(msg, msg, Messages);

                //return 1;
            }

            return 1;
        }
    }
}