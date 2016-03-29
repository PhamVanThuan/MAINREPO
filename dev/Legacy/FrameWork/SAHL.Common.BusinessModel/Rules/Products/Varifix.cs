using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.DataAccess;
using System.Data;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    [RuleDBTag("ProductVarifixApplicationMinLoanAmount",
        "ProductVarifixApplicationMinLoanAmount",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixApplicationMinLoanAmount")]
    [RuleParameterTag(new string[] { "@MinLoanAmount,200000,9" })]
    [RuleInfo]
    public class ProductVarifixApplicationMinLoanAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixApplicationMinLoanAmount rule expects a Domain object to be passed.");

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (application == null)
                throw new ArgumentException("The ProductVarifixApplicationMinLoanAmount rule expects the following objects to be passed: IApplicationMortgageLoan.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            if (application is IApplicationFurtherLending || !(application.CurrentProduct is IApplicationProductVariFixLoan))
                return 1;


            //if (application.CurrentProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan)
            //{
            IApplicationProductVariFixLoan applicationProductVariFixLoan = (IApplicationProductVariFixLoan)application.CurrentProduct;

            if (applicationProductVariFixLoan != null)
            {
                double minLoanAgreementAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

                if (applicationProductVariFixLoan.LoanAgreementAmount < minLoanAgreementAmount)
                {
                    string errorMessage = String.Format("The Minimum Loan Amount is {0}.", minLoanAgreementAmount);
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }
            //}
            return 0;
        }
    }

    [RuleDBTag("ProductVarifixApplicationMaxLTV",
        "ProductVarifixApplicationMaxLTV",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixApplicationMaxLTV")]
    [RuleParameterTag(new string[] { "@MaxLTV,90,9" })]
    [RuleInfo]
    public class ProductVarifixApplicationMaxLTV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixApplicationMaxLTV rule expects a Domain object to be passed.");

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (application == null)
                throw new ArgumentException("The ProductVarifixApplicationMaxLTV rule expects the following objects to be passed: IApplicationMortgageLoan.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            IApplicationProductVariFixLoan applicationProductVariFixLoan = (IApplicationProductVariFixLoan)application.CurrentProduct;
            // Get the LoanAgreementAmount
            double maxLTV = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = applicationProductVariFixLoan as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
            {
                if (supportsVariableLoanApplicationInformation.VariableLoanInformation.LTV > maxLTV)
                {
                    string errorMessage = String.Format("The maximum LTV allowed on a Varifix loan is {0}%.", maxLTV);
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ProductVarifixApplicationTerm",
        "ProductVarifixApplicationTerm",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixApplicationTerm")]
    [RuleInfo]
    public class ProductVarifixApplicationTerm : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixApplicationTerm rule expects a Domain object to be passed.");

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (application == null)
                throw new ArgumentException("The ProductVarifixApplicationTerm rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion

            IApplicationProductVariFixLoan applicationProductVariFixLoan = (IApplicationProductVariFixLoan)application.CurrentProduct;

            if (applicationProductVariFixLoan.Term > (20 * 12))
            {
                string errorMessage = String.Format("The term of a Varifix loan cannot be extended beyond 20 years.");
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ProductVarifixApplicationFixedMinimum",
        "ProductVarifixApplicationFixedMinimum",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixApplicationFixedMinimum")]
    [RuleParameterTag(new string[] { "@MinFixedPortion,50000.0,9" })]
    [RuleInfo]
    public class ProductVarifixApplicationFixedMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixApplicationFixedMinimum rule expects a Domain object to be passed.");

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            if (application == null)
                throw new ArgumentException("The ProductVarifixApplicationFixedMinimum rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion

            if (application is IApplicationFurtherLending || !(application.CurrentProduct is IApplicationProductVariFixLoan))
                return 1;

            double minFixedPortion = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            IApplicationProductVariFixLoan applicationProductVariFixLoan = (IApplicationProductVariFixLoan)application.CurrentProduct;
            if (applicationProductVariFixLoan != null)
            {
                // Get the full loan amountconditrio
                double loanAgreementAmount;
                ISupportsVariableLoanApplicationInformation suppVLAI = applicationProductVariFixLoan as ISupportsVariableLoanApplicationInformation;

                if (suppVLAI.VariableLoanInformation.LoanAgreementAmount.HasValue)
                    loanAgreementAmount = suppVLAI.VariableLoanInformation.LoanAgreementAmount.Value;
                else
                {
                    loanAgreementAmount = suppVLAI.VariableLoanInformation.LoanAmountNoFees.HasValue ? suppVLAI.VariableLoanInformation.LoanAmountNoFees.Value : 0D;
                    if (application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
                        loanAgreementAmount += suppVLAI.VariableLoanInformation.FeesTotal.HasValue ? suppVLAI.VariableLoanInformation.FeesTotal.Value : 0D;

                }

                //loanAgreementAmount = (suppVLAI.VariableLoanInformation.LoanAmountNoFees.HasValue ? suppVLAI.VariableLoanInformation.LoanAmountNoFees.Value : 0D);

                //if (suppVLAI.VariableLoanInformation.LoanAgreementAmount.HasValue)
                //    loanAgreementAmount = suppVLAI.VariableLoanInformation.LoanAgreementAmount.Value;
                //else
                //    throw new ArgumentException("The ProductVarifixApplicationFixedMinimum rule expects LoanAgreementAmount to have a value.");
               
                // Get the varifix split
                double fixedLoanAmount;
                ISupportsVariFixApplicationInformation suppFAI = applicationProductVariFixLoan as ISupportsVariFixApplicationInformation;
                fixedLoanAmount = suppFAI.VariFixInformation.FixedPercent * loanAgreementAmount;

                if (fixedLoanAmount < minFixedPortion)
                {
                    string errorMessage = String.Format("The fixed portion may not be less than {0}.", minFixedPortion);
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }


    /// <summary>
    /// Called on OptIn.
    /// </summary>
    [RuleDBTag("ProductVarifixAccountOptInOpenAccount",
        "ProductVarifixAccountOptInOpenAccount",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixAccountOptInOpenAccount")]
    [RuleInfo]
    public class ProductVarifixAccountOptInOpenAccount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixAccountOptInOpenAccount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ProductVarifixAccountOptInOpenAccount rule expects the following objects to be passed: IAccount.");

            #endregion

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            if (mortgageLoanAccount == null
                || mortgageLoanAccount.AccountStatus.Key != (int)AccountStatuses.Open)
            {
                string errorMessage = "The client must have an open loan.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    /// <summary>
    /// Called on OptIn.
    /// </summary>
    [RuleDBTag("ProductVarifixAccountOptInFurtherLoan",
        "ProductVarifixAccountOptInFurtherLoan",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixAccountOptInFurtherLoan")]
    [RuleInfo]
    public class ProductVarifixAccountOptInFurtherLoan : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixAccountOptInFurtherLoan rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ProductVarifixAccountOptInFurtherLoan rule expects the following object to be passed: IAccount.");

            #endregion

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            foreach (IApplication application in mortgageLoanAccount.Applications)
            {
                if (application.ApplicationStatus.Key == (int)OfferStatuses.Open
                    && application is IApplicationFurtherLoan)
                {
                    string errorMessage = "Further lending may not be in progress.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    break;
                }
            }

            return 0;
        }
    }

    /// <summary>
    /// Called on OptIn.
    /// </summary>
    [RuleDBTag("ProductVarifixAccountOptInReadvance",
        "ProductVarifixAccountOptInReadvance",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixAccountOptInReadvance")]
    [RuleInfo]
    public class ProductVarifixAccountOptInReadvance : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixAccountOptInReadvance rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ProductVarifixAccountOptInReadvance rule expects the following object to be passed: IAccount.");

            #endregion

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            foreach (IApplication application in mortgageLoanAccount.Applications)
            {
                if (application.ApplicationStatus.Key == (int)OfferStatuses.Open
                    && application is IApplicationReAdvance)
                {
                    string errorMessage = "Further lending may not be in progress.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    break;
                }
            }

            return 0;
        }
    }

    /// <summary>
    /// Called on OptIn.
    /// </summary>
    [RuleDBTag("ProductVarifixAccountOptInArrears",
        "ProductVarifixAccountOptInArrears",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixAccountOptInArrears")]
    [RuleInfo]
    public class ProductVarifixAccountOptInArrears : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixAccountOptInArrears rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ProductVarifixAccountOptInArrears rule expects the following object to be passed: IAccount.");

            #endregion

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            if (mortgageLoanAccount.InstallmentSummary.MonthsInArrears > 1)
            {
                string errorMessage = "The client may not be more than one month in arrears.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;

        }
    }

    /// <summary>
    /// Called on OptIn
    /// </summary>
    [RuleDBTag("ProductVarifixAccountOptInDebtCounselling",
       "The loan cannot currently be under debt counseling",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixAccountOptInDebtCounselling")]
    [RuleInfo]
    public class ProductVarifixAccountOptInDebtCounselling : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixAccountOptInDebtCounselling rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ProductVarifixAccountOptInDebtCounselling rule expects the following objects to be passed: IAccount.");

            #endregion

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            bool underCancellation = false;
            
            underCancellation = stageDefinitionRepository.CheckCompositeStageDefinition(mortgageLoanAccount.Key, stageDefinitionRepository.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.DebtCounselling, (int)StageDefinitions.DebtCounselling).Key);
            if (underCancellation)
            {
                string errorMessage = "The Account is under Debt Councelling and therefore may not opt into Varifix.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ProductVarifixAccountOptInCapApplication",
       "The loan cannot have an Open CAP 2 Application",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixAccountOptInCapApplication")]
    [RuleInfo]
    public class ProductVarifixAccountOptInCapApplication : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixAccountOptInCapApplication rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ProductVarifixAccountOptInCapApplication rule expects the following objects to be passed: IAccount.");

            #endregion

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            foreach (IApplication  application in mortgageLoanAccount.Applications)
            {
                foreach (IApplicationInformation applicationInformation in application.ApplicationInformations)
                {
                    foreach (var applicationInfoFinancialAdjustments in applicationInformation.ApplicationInformationFinancialAdjustments)
                    {
                        if (applicationInfoFinancialAdjustments.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.CAP2)
                        {
                            string errorMessage = "The loan cannot have an Open CAP 2 Application.";
                            AddMessage(errorMessage, errorMessage, Messages);
                        }
                    }
                }
            }

            return 0;
        }
    }

    /// <summary>
    /// To be run as part of the opt in procedure.
    /// </summary>
    [RuleDBTag("ProductVarifixOptInLoanTransaction",
   "ProductVarifixOptInLoanTransaction",
      "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixOptInLoanTransaction")]
    [RuleInfo]
    public class ProductVarifixOptInLoanTransaction : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixOptInLoanTransaction rule expects a Domain object to be passed.");

            IAccount account = Parameters[0] as IAccount;
            IApplication application = Parameters[0] as IApplication;

            if (account == null && application == null)
                throw new ArgumentException("The ProductVarifixOptInLoanTransaction rule expects one of the following objects to be passed: IAccount or IApplication.");

            if (account == null && application.Account != null)
                account = application.Account;

            // int accountKey = 0;
            // bool isFurtherLoan = false;

            //if (Parameters[0] is IAccount)
            //    account = Parameters[0] as IAccount;

            //if (Parameters[0] is IApplication)
            //{
                //if (app.Account != null)
                //{
                //    accountKey = app.Account.Key;
                //    isFurtherLoan = (app is IApplicationReAdvance);
                //    if (!isFurtherLoan)
                //        isFurtherLoan = (app is IApplicationFurtherAdvance);
                //    if (!isFurtherLoan)
                //        isFurtherLoan = (app is IApplicationFurtherLoan);
                //}
            //}

            //need to be able to check if an account supports further loans
            //done by passing in an extra parameter when checking from the FL calculator
            //if (Parameters.Length > 1 && isFurtherLoan == false)
            //{
            //    isFurtherLoan = (bool)Parameters[1];
            //}
            #endregion

            if (account == null || (account.AccountStatus.Key != (int)AccountStatuses.Open && account.AccountStatus.Key != (int)AccountStatuses.Dormant))
                return 1;

            IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");
            try
            {
                string sqlQuery = UIStatementRepository.GetStatement("Rules.Products", "ProductVarifixOptInLoanTransaction");
                ParameterCollection prms = new ParameterCollection();
                Helper.AddIntParameter(prms, "@AccountKey", account.Key);
                //Helper.AddBitParameter(prms, "@isFurtherLoan", isFurtherLoan);
                object o = Helper.ExecuteScalar(con, sqlQuery, prms);
                if (Convert.ToInt16(o) == 1)
                {
                    string errorMessage = "Further lending can not be started because the client has initiated an opt in into Varifix. The opt in must be completed or cancelled before you can continue.";
                    //if (isFurtherLoan)
                    //    errorMessage = "Further loans can not be done because the client has initiated an opt in/out of Varifix. The opt in/out must be completed or cancelled before you can continue.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }
                    return 1;
            }
            finally
            {
                if (con != null)
                    con.Dispose();
            }

            #region Old Code
            /*ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");
            DataTable DT = new DataTable();
            
            //Further Loans can not be done even if opting out of VF
            // check e-works for opt in/outs and OfferInformationVarifixLoan for originated VF loans
            string query = String.Format("select hd.LoanNumber from [e-work]..HelpDesk hd where hd.ActiveFolder = -1 " 
                + "{1} and hd.LoanNumber = {0} " 
                + "UNION ALL "
                + "select o.ReservedAccountKey from [2am].dbo.offer o "
                + "join [2am].dbo.OfferInformation oi on o.OfferKey = oi.OfferKey "
                + "join [2am].dbo.OfferInformationVarifixLoan vf on oi.OfferInformationKey = vf.OfferInformationKey "
                + "where oi.OfferInformationTypeKey in (1, 3) and o.OfferTypeKey in (6, 7, 8) "
                + "and ConversionStatus in (0, 1) and o.ReservedAccountKey = {0}; ", accountKey, !isFurtherLoan ? " and bClientOptOut != -1 " : " ");

            Helper.FillFromQuery(DT, query, con, parameters);
            
            if (DT.Rows.Count > 0)
            {
                string errorMessage = "Further lending can not be started because the client has initiated an opt in into Varifix. The opt in must be completed or cancelled before you can continue.";
                if (isFurtherLoan)
                    errorMessage = "Further loans can not be done because the client has initiated an opt in/out of Varifix. The opt in/out must be completed or cancelled before you can continue.";
                
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
            */
            #endregion
        }
    }

    /// <summary>
    /// To be run as part of the opt in procedure.
    /// </summary>
    [RuleDBTag("ProductVarifixOptInFlag",
    "ProductVarifixOptInFlag",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ProductVarifixOptInFlag", false)]
    [RuleInfo]
    public class ProductVarifixOptInFlag : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProductVarifixOptInFlag rule expects a Domain object to be passed.");

            IAccount account = Parameters[0] as IAccount;
            IApplication application = Parameters[0] as IApplication;

            if (account == null && application == null)
                throw new ArgumentException("The ProductVarifixOptInFlag rule expects one of the following objects to be passed: IAccount or IApplication.");

            if (account == null && application.Account != null)
                account = application.Account;

            //int accountKey = 0;
            //bool isFurtherLoan = false;

            //if (Parameters[0] is IAccount)
            //{
            //    IAccount account = Parameters[0] as IAccount;
            //    accountKey = account.Key;
            //}

            //if (Parameters[0] is IApplication)
            //{
            //    IApplication app = Parameters[0] as IApplication;
            //    if (app.Account != null)
            //    {
            //        accountKey = app.Account.Key;
            //        isFurtherLoan = (app is IApplicationReAdvance);
            //        if (!isFurtherLoan)
            //            isFurtherLoan = (app is IApplicationFurtherAdvance);
            //        if (!isFurtherLoan)
            //            isFurtherLoan = (app is IApplicationFurtherLoan);
            //    }
            //}

            ////need to be able to check if an account supports further loans
            ////done by passing in an extra parameter when checking from the FL calculator
            //if (Parameters.Length > 1 && isFurtherLoan == false)
            //{
            //    isFurtherLoan = (bool)Parameters[1];
            //}
            #endregion

            if (account == null || (account.AccountStatus.Key != (int)AccountStatuses.Open && account.AccountStatus.Key != (int)AccountStatuses.Dormant))
                return 1;

            IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");
            try
            {
                string sqlQuery = UIStatementRepository.GetStatement("Rules.Products", "ProductVarifixOptInLoanTransaction");
                ParameterCollection prms = new ParameterCollection();
                Helper.AddIntParameter(prms, "@AccountKey", account.Key);
                //Helper.AddBitParameter(prms, "@isFurtherLoan", isFurtherLoan);

                object o = Helper.ExecuteScalar(con, sqlQuery, prms);

                if (Convert.ToInt16(o) == 1)
                {
                    string errorMessage = "Opt In to Varifix pending.";

                    AddMessage(errorMessage, errorMessage, Messages);
                    return 1;
                }
                return 0;
            }
            finally
            {
                if (con != null)
                    con.Dispose();
            }
        }
    }

}
