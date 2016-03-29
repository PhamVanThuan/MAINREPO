using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.Application.FurtherLending
{
    [RuleDBTag("ApplicationFurtherLendingAccountStatus",
            "ApplicationFurtherLendingAccountStatus",
            "SAHL.Rules.DLL",
          "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingAccountStatus")]
    [RuleInfo]
    public class ApplicationFurtherLendingAccountStatus : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingAccountStatus rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The ApplicationFurtherLendingAccountStatus rule expects the following objects to be passed: IApplicationFurtherLending.");

            #endregion Check for allowed object type(s)

            IApplicationFurtherLending applicationFurtherLending = Parameters[0] as IApplicationFurtherLending;

            if (!(applicationFurtherLending.Account != null
                && applicationFurtherLending.Account.AccountStatus.Key == (int)AccountStatuses.Open))
            {
                string errorMessage = "A Further Loan Application can only be processed on an open loan.";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingAccountForeClosure",
        "ApplicationFurtherLendingAccountForeClosure",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingAccountForeClosure")]
    [RuleInfo]
    public class ApplicationFurtherLendingAccountForeClosure : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingAccountForeClosure rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The ApplicationFurtherLendingAccountForeClosure rule expects one of the following objects to be passed: IAccount or IApplicationFurtherLending.");

            #endregion Check for allowed object type(s)

            int accountKey = 0;
            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            if (Parameters[0] is IApplicationFurtherLending)
            {
                IApplicationFurtherLending app = Parameters[0] as IApplicationFurtherLending;
                accountKey = app.Account.Key;
            }

            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IReadOnlyEventList<IDetail> details = accRepo.GetDetailByAccountKeyAndDetailType(accountKey, (int)DetailTypes.ForeclosureUnderway);
            if (details != null && details.Count > 0)
            {
                string errorMessage = "A Further Loan Application can not be processed on a loan under ForeClosure.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingAccountCancellation",
        "ApplicationFurtherLendingAccountCancellation",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingAccountCancellation")]
    [RuleInfo]
    public class ApplicationFurtherLendingAccountCancellation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingAccountCancellation rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The ApplicationFurtherLendingAccountCancellation rule expects one of the following objects to be passed: IAccount or IApplicationFurtherLending.");

            #endregion Check for allowed object type(s)

            int accountKey = 0;
            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            if (Parameters[0] is IApplicationFurtherLending)
            {
                IApplicationFurtherLending app = Parameters[0] as IApplicationFurtherLending;
                accountKey = app.Account.Key;
            }

            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IReadOnlyEventList<IDetail> details = accRepo.GetDetailByAccountKeyAndDetailType(accountKey, (int)DetailTypes.UnderCancellation);
            if (details != null && details.Count > 0)
            {
                string errorMessage = "A Further Loan Application can not be processed on a loan under Cancellation.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingSPV",
        "ApplicationFurtherLendingSPV",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingSPV")]
    [RuleInfo]
    public class ApplicationFurtherLendingSPV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingSPV rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The ApplicationFurtherLendingSPV rule expects the following objects to be passed: IApplicationFurtherLending.");

            #endregion Check for allowed object type(s)

            IApplicationFurtherLending applicationFurtherLending = Parameters[0] as IApplicationFurtherLending;

            // Get the LTV and PTI
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = applicationFurtherLending.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            foreach (ISPVMandate spvMandate in supportsVariableLoanApplicationInformation.VariableLoanInformation.SPV.SPVMandates)
            {
                if (supportsVariableLoanApplicationInformation.VariableLoanInformation.LTV > spvMandate.MaxLTV)
                {
                    string errorMessage = String.Format("The maximum LTV for the current SPV ({0}%) has been exceeded.", spvMandate.MaxLTV);
                    AddMessage(errorMessage, errorMessage, Messages);
                }

                if (supportsVariableLoanApplicationInformation.VariableLoanInformation.PTI > spvMandate.MaxPTI)
                {
                    string errorMessage = String.Format("The maximum PTI for the current SPV ({0}%) has been exceeded.", spvMandate.MaxPTI);
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationFurtherAdvanceLoansNewValuationRequired",
        "ApplicationFurtherAdvanceLoansNewValuationRequired",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherAdvanceLoansNewValuationRequired")]
    [RuleInfo]
    public class ApplicationFurtherAdvanceLoansNewValuationRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherAdvanceLoansNewValuationRequired rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationFurtherAdvance))
                throw new ArgumentException("The ApplicationFurtherAdvanceLoansNewValuationRequired rule expects the following objects to be passed: IApplicationFurtherAdvance.");

            #endregion Check for allowed object type(s)

            IApplicationFurtherAdvance applicationFurtherAdvance = Parameters[0] as IApplicationFurtherAdvance;

            DateTime latestValuationDate = DateTime.MinValue;
            foreach (IValuation valuation in applicationFurtherAdvance.Property.Valuations)
            {
                if (valuation.ValuationDate > latestValuationDate)
                    latestValuationDate = valuation.ValuationDate;
            }

            if (latestValuationDate.AddYears(2) < DateTime.Today)
            {
                string errorMessage = "The Property Valuation is over two years old. A new Valuation is required.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationReAdvanceLoansNewValuationRequired",
        "ApplicationReAdvanceLoansNewValuationRequired",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationReAdvanceLoansNewValuationRequired")]
    [RuleInfo]
    public class ApplicationReAdvanceLoansNewValuationRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationReAdvanceLoansNewValuationRequired rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationReAdvance))
                throw new ArgumentException("The ApplicationReAdvanceLoansNewValuationRequired rule expects the following objects to be passed: IApplicationReAdvance.");

            #endregion Check for allowed object type(s)

            IApplicationReAdvance applicationReAdvance = Parameters[0] as IApplicationReAdvance;

            DateTime latestValuationDate = DateTime.MinValue;
            foreach (IValuation valuation in applicationReAdvance.Property.Valuations)
            {
                if (valuation.ValuationDate > latestValuationDate)
                    latestValuationDate = valuation.ValuationDate;
            }

            if (latestValuationDate.AddYears(3) < DateTime.Today)
            {
                string errorMessage = "The Property Valuation is over two years old. A new Valuation is required.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    /// <summary>
    /// This might only need to be called at a certain point in workflow.
    /// Otherwise the loan balance might change before the final approval.
    /// </summary>
    [RuleDBTag("ApplicationFurtherLoanAccountBalance",
        "ApplicationFurtherLoanAccountBalance",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLoanAccountBalance")]
    [RuleInfo]
    public class ApplicationFurtherLoanAccountBalance : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLoanAccountBalance rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationFurtherLoan))
                throw new ArgumentException("The ApplicationFurtherLoanAccountBalance rule expects the following objects to be passed: IApplicationFurtherLoan.");

            #endregion Check for allowed object type(s)

            IApplicationFurtherLoan applicationFurtherLoan = Parameters[0] as IApplicationFurtherLoan;

            // Get the bond amount.
            double bondLoanAgreementAmount = 0.0;
            double furtherLoanAmount = 0.0;
            DateTime minBondRegistrationDate = DateTime.MinValue;
            double loanCurrentBalance = 0.0;

            foreach (IFinancialService financialService in applicationFurtherLoan.Account.FinancialServices)
            {
                if (financialService.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan
                    && financialService.AccountStatus.Key == (int)AccountStatuses.Open)
                {
                    // Found the bond - get teh bond value.
                    IMortgageLoan mortgageLoan = financialService as IMortgageLoan;

                    foreach (IBond bond in mortgageLoan.Bonds)
                    {
                        // It's normally 2 recs only - I'll sort it manually
                        if (bond.BondRegistrationDate > minBondRegistrationDate)
                        {
                            bondLoanAgreementAmount = bond.BondLoanAgreementAmount;
                            minBondRegistrationDate = bond.BondRegistrationDate;
                        }
                    }
                }
            }

            // Get the loan current balance
            IMortgageLoanAccount mortgageLoanAccount = applicationFurtherLoan.Account as IMortgageLoanAccount;
            if (mortgageLoanAccount != null)
                loanCurrentBalance = mortgageLoanAccount.LoanCurrentBalance;

            // Get the furtherLoanAmount
            furtherLoanAmount = applicationFurtherLoan.RequestedCashAmount.Value;
            if ((loanCurrentBalance + furtherLoanAmount) <= bondLoanAgreementAmount)
            {
                string errorMessage = "The Further Loan requested must take the Current Balance over the current Agreement Bond Amount.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("AccountDebtCounseling",
      "A Further Lending Application cannot be processed on a loan that is under going Debt Counseling",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDebtCounseling")]
    [RuleInfo]
    public class AccountDebtCounseling : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountDebtCounseling rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplication))
                throw new ArgumentException("The AccountDebtCounseling rule expects one of the following objects to be passed: IAccount or IApplication.");

            #endregion Check for allowed object type(s)

            bool underDebtCounselling = false;
            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                underDebtCounselling = acc.UnderDebtCounselling;
            }

            if (Parameters[0] is IApplication)
            {
                IApplication app = Parameters[0] as IApplication;
                if (app.Account != null && app.Account.UnderDebtCounselling)
                {
                    underDebtCounselling = app.Account.UnderDebtCounselling;
                }
            }

            if (underDebtCounselling)
            {
                string errorMessage = "This Account is undergoing Debt Counselling.";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }

        #region Old Code - #Ticket13202

        //    #region Check for allowed object type(s)
        //    if (Parameters.Length == 0)
        //        throw new ArgumentException("The AccountDebtCounseling rule expects a Domain object to be passed.");

        //    if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
        //        throw new ArgumentException("The AccountDebtCounseling rule expects one of the following objects to be passed: IAccount or IApplicationFurtherLending.");

        //    #endregion

        //    IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
        //    int accountKey = 0;

        //    if (Parameters[0] is IAccount)
        //    {
        //        IAccount acc = Parameters[0] as IAccount;
        //        accountKey = acc.Key;
        //    }

        //    if (Parameters[0] is IApplicationFurtherLending)
        //    {
        //        IApplicationFurtherLending app = Parameters[0] as IApplicationFurtherLending;
        //        accountKey = app.Account.Key;
        //    }

        //    bool underReview = false;
        //    int sdsdgKey = stageDefinitionRepository.GetStageDefinitionStageDefinitionGroupKey((int)StageDefinitionGroups.DebtCounselling, (int)StageDefinitions.DebtCounselling);
        //    underReview = stageDefinitionRepository.CheckCompositeStageDefinition(accountKey, sdsdgKey);
        //    if (underReview)
        //    {
        //        string errorMessage = "This Account is undergoing Debt Counselling.";
        //        AddMessage(errorMessage, errorMessage, Messages);
        //    }

        //    return 0;
        //}

        #endregion Old Code - #Ticket13202
    }

    [RuleDBTag("AccountDebtCounselingQuickCash",
    "A Further Lending Application cannot be processed on a loan that is under going Debt Counseling",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDebtCounselingQuickCash")]
    [RuleInfo]
    public class AccountDebtCounselingQuickCash : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountDebtCounselingQuickCash rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The AccountDebtCounselingQuickCash rule expects one of the following objects to be passed: IAccount or IApplicationFurtherLending.");

            #endregion Check for allowed object type(s)

            bool underDebtCounselling = false;
            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                underDebtCounselling = acc.UnderDebtCounselling;
            }

            if (Parameters[0] is IApplication)
            {
                IApplication app = Parameters[0] as IApplication;
                underDebtCounselling = app.Account.UnderDebtCounselling;
            }

            if (underDebtCounselling)
            {
                string errorMessage = "This Account is undergoing Debt Counselling.";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("AccountUnderForeClosure",
    "AccountUnderForeClosure",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountUnderForeClosure")]
    [RuleInfo]
    public class AccountUnderForeClosure : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountUnderForeClosure rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The AccountUnderForeClosure rule expects one of the following objects to be passed: IAccount or IApplication.");

            #endregion Check for allowed object type(s)

            int accountKey = 0;
            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            if (Parameters[0] is IApplication)
            {
                IApplication app = Parameters[0] as IApplication;
                if (app.Account != null)
                    accountKey = app.Account.Key;
                else if (app.ReservedAccount != null)
                    accountKey = app.ReservedAccount.Key;
                else
                    accountKey = -1;
            }

            if (accountKey != -1)
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                IReadOnlyEventList<IDetail> details = accRepo.GetDetailByAccountKeyAndDetailType(accountKey, (int)DetailTypes.ForeclosureUnderway);

                //foreclosure exists
                if (details != null && details.Count > 0)
                {
                    DateTime? forecloseDate = null; //get the date
                    foreach (IDetail dt in details)
                    {
                        if (forecloseDate.HasValue == false || dt.DetailDate > forecloseDate)
                            forecloseDate = dt.DetailDate;
                    }

                    IReadOnlyEventList<IDetail> detailLAS = accRepo.GetDetailByAccountKeyAndDetailType(accountKey, (int)DetailTypes.LegalActionStopped);

                    // LegalActionStopped exists, check the dates
                    DateTime? lasDate = null; //get the date
                    if (detailLAS != null && detailLAS.Count > 0)
                    {
                        foreach (IDetail dt in detailLAS)
                        {
                            if (lasDate.HasValue == false || dt.DetailDate > lasDate)
                                lasDate = dt.DetailDate;
                        }
                    }

                    //we have a foreclosure, if there is no LegalActionStopped with a date later than Foreclosure, return an error
                    if (lasDate.HasValue == false || forecloseDate.Value > lasDate.Value)
                    {
                        string errorMessage = "This Account under ForeClosure.";
                        AddMessage(errorMessage, errorMessage, Messages);
                        return 1;
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("AccountDetailTypeCheck",
    "AccountDetailTypeCheck",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeCheck")]
    [RuleInfo]
    public class AccountDetailTypeCheck : BusinessRuleBase
    {
        private ICastleTransactionsService castleTransactionService;
        private IUIStatementService uiStatementService;

        public AccountDetailTypeCheck(ICastleTransactionsService castleTransactionService, IUIStatementService uiStatementService)
        {
            this.castleTransactionService = castleTransactionService;
            this.uiStatementService = uiStatementService;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountDetailTypeCheck rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The AccountDetailTypeCheck rule expects one of the following objects to be passed: IAccount or IApplication.");

            #endregion Check for allowed object type(s)

            int accountKey = 0;
            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            if (Parameters[0] is IApplication)
            {
                IApplication app = Parameters[0] as IApplication;
                if (app.Account != null)
                    accountKey = app.Account.Key;
                else if (app.ReservedAccount != null)
                    accountKey = app.ReservedAccount.Key;
                else
                    accountKey = -1;
            }

            if (accountKey != -1)
            {
                string query = uiStatementService.GetStatement("Rules.FurtherLending", "AccountDetailTypeCheck");

                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@AccountKey", accountKey));

                DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, Databases.TwoAM, prms);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        AddMessage("Further Lending can not be processed because of Detail Types.", "Further Lending can not be processed because of Detail Types.", Messages);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AddMessage(dr[0].ToString(), dr[0].ToString(), Messages);
                        }

                        return 1;
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("AccountDetailTypeWarning",
    "Shows warnings but does not prevent a user from initiating Further Lending",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeWarning")]
    [RuleInfo]
    public class AccountDetailTypeWarning : BusinessRuleBase
    {
        private ICastleTransactionsService castleTransactionService;
        private IUIStatementService uiStatementService;

        public AccountDetailTypeWarning(ICastleTransactionsService castleTransactionService, IUIStatementService uiStatementService)
        {
            this.castleTransactionService = castleTransactionService;
            this.uiStatementService = uiStatementService;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountDetailTypeWarning rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The AccountDetailTypeWarning rule expects one of the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            int accountKey = 0;
            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            if (accountKey != -1)
            {
                string query = uiStatementService.GetStatement("Rules.FurtherLending", "AccountDetailTypeWarning");

                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@AccountKey", accountKey));

                DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, Databases.TwoAM, prms);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        AddMessage("The following detail types are loaded against the loan.", "The following detail types are loaded against the loan.", Messages);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AddMessage(dr[0].ToString(), dr[0].ToString(), Messages);
                        }

                        return 1;
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("AccountDetailTypeVarifixOptOut90DayPendingHoldCheck",
    "Varifix opt out 90 day pending hold is loaded on this loan. The application amount must be lower than the loan agreement amount.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeVarifixOptOut90DayPendingHoldCheck")]
    [RuleInfo]
    public class AccountDetailTypeVarifixOptOut90DayPendingHoldCheck : BusinessRuleBase
    {
        private ICastleTransactionsService castleTransactionService;
        private IUIStatementService uiStatementService;

        public AccountDetailTypeVarifixOptOut90DayPendingHoldCheck(ICastleTransactionsService castleTransactionService, IUIStatementService uiStatementService)
        {
            this.castleTransactionService = castleTransactionService;
            this.uiStatementService = uiStatementService;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountDetailTypeVarifixOptOut90DayPendingHoldCheck rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The AccountDetailTypeVarifixOptOut90DayPendingHoldCheck rule expects one of the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IAccount account = null;
            IApplicationReAdvance appReAdvance = null;
            IApplicationFurtherAdvance appFurtherAdvance = null;
            IApplicationFurtherLoan appFurtherLoan = null;

            if (Parameters[0] is IAccount)
            {
                account = Parameters[0] as IAccount;
            }

            if (Parameters[1] is IApplicationReAdvance)
            {
                appReAdvance = Parameters[1] as IApplicationReAdvance;
            }

            if (Parameters[2] is IApplicationFurtherAdvance)
            {
                appFurtherAdvance = Parameters[2] as IApplicationFurtherAdvance;
            }

            if (Parameters[3] is IApplicationFurtherLoan)
            {
                appFurtherLoan = Parameters[3] as IApplicationFurtherLoan;
            }

            if (IsTotalAppAmtGreaterThanLoanAgreementAmount(account, appReAdvance, appFurtherAdvance, appFurtherLoan) && account != null)
            {
                string query = uiStatementService.GetStatement("Rules.FurtherLending", "AccountDetailTypeVarifixOptOut90DayPendingHoldCheck");

                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@AccountKey", account.Key));

                DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, Databases.TwoAM, prms);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string message = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            message = string.Format("{0} detail type is loaded against the loan.", dr[0].ToString());
                            AddMessage(message, message, Messages);
                        }
                        message = "The application amount must be lower than the loan agreement amount.";
                        AddMessage(message, message, Messages);
                    }
                    return 1;
                }
            }
            return 0;
        }

        private bool IsTotalAppAmtGreaterThanLoanAgreementAmount(IAccount account, IApplicationReAdvance appReAdvance, IApplicationFurtherAdvance appFurtherAdvance, IApplicationFurtherLoan appFurtherLoan)
        {
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
            double accCurrBalance = mortgageLoanAccount.LoanCurrentBalance;

            double readvanceAmount = 0D;
            double furtherAdvanceAmount = 0D;
            double furtherLoanAmount = 0D;

            if (appReAdvance != null)
            {
                ISupportsVariableLoanApplicationInformation supportsVariableLoanAppInfo = appReAdvance.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan variableLoanInformation = supportsVariableLoanAppInfo.VariableLoanInformation;
                readvanceAmount = variableLoanInformation.LoanAmountNoFees.HasValue ? variableLoanInformation.LoanAmountNoFees.Value : 0;
            }
            if (appFurtherAdvance != null)
            {
                ISupportsVariableLoanApplicationInformation supportsVariableLoanAppInfo = appFurtherAdvance.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan variableLoanInformation = supportsVariableLoanAppInfo.VariableLoanInformation;
                furtherAdvanceAmount = variableLoanInformation.LoanAmountNoFees.HasValue ? variableLoanInformation.LoanAmountNoFees.Value : 0;
            }
            if (appFurtherLoan != null)
            {
                ISupportsVariableLoanApplicationInformation supportsVariableLoanAppInfo = appFurtherLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan variableLoanInformation = supportsVariableLoanAppInfo.VariableLoanInformation;
                furtherLoanAmount = variableLoanInformation.LoanAmountNoFees.HasValue ? variableLoanInformation.LoanAmountNoFees.Value : 0;
            }

            double total = accCurrBalance + readvanceAmount + furtherAdvanceAmount + furtherLoanAmount;
            double bondLoanAgreementAmount = GetMortgageLoanAgreementAmount(account);

            return total > bondLoanAgreementAmount;
        }

        public double GetMortgageLoanAgreementAmount(IAccount account)
        {
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
            IMortgageLoan variableMortgageLoan = mortgageLoanAccount.SecuredMortgageLoan;
            return variableMortgageLoan.SumBondLoanAgreementAmounts();
        }
    }

    [RuleDBTag("AccountDebtCounselingLossControl",
    "A Further Lending Application cannot be processed on a loan that is under going Debt Counseling",
    "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDebtCounselingLossControl")]
    [RuleInfo]
    public class AccountDebtCounselingLossControl : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountDebtCounselingLossControl rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The AccountDebtCounselingLossControl rule expects one of the following objects to be passed: IAccount or IApplicationFurtherLending.");

            #endregion Check for allowed object type(s)

            IStageDefinitionRepository SDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            int accountKey = 0;

            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            if (Parameters[0] is IApplicationFurtherLending)
            {
                IApplicationFurtherLending app = Parameters[0] as IApplicationFurtherLending;
                accountKey = app.Account.Key;
            }

            // If we have gone into QC debt counselling more times than we have come out,
            // then we can be sure that we are in debt counselling
            IList<IStageTransition> transistions = SDRepo.GetStageTransitionsByGenericKey(accountKey);
            if (transistions != null && transistions.Count > 0)
            {
                int count = 0;
                foreach (IStageTransition st in transistions)
                {
                    if (st.StageDefinitionStageDefinitionGroup.Key == (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlIn)
                        count += 1;

                    if (st.StageDefinitionStageDefinitionGroup.Key == (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlOut)
                        count -= 1;
                }
                if (count > 0)
                {
                    string errorMessage = "This Account is undergoing Debt Counselling.";
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("AccountDebtCounselingLossControlExternal",
    "A Further Lending Application cannot be processed on a loan that is under going Debt Counseling",
    "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDebtCounselingLossControlExternal")]
    [RuleInfo]
    public class AccountDebtCounselingLossControlExternal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountDebtCounselingLossControlExternal rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount) && !(Parameters[0] is IApplicationFurtherLending))
                throw new ArgumentException("The AccountDebtCounselingLossControlExternal rule expects one of the following objects to be passed: IAccount or IApplicationFurtherLending.");

            #endregion Check for allowed object type(s)

            IStageDefinitionRepository SDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            int accountKey = 0;

            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            if (Parameters[0] is IApplicationFurtherLending)
            {
                IApplicationFurtherLending app = Parameters[0] as IApplicationFurtherLending;
                accountKey = app.Account.Key;
            }

            // If we have gone into QC debt counselling more times than we have come out,
            // then we can be sure that we are in debt counselling
            IList<IStageTransition> transistions = SDRepo.GetStageTransitionsByGenericKey(accountKey);
            if (transistions != null && transistions.Count > 0)
            {
                int count = 0;
                foreach (IStageTransition st in transistions)
                {
                    if (st.StageDefinitionStageDefinitionGroup.Key == (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlExternalIn)
                        count += 1;

                    if (st.StageDefinitionStageDefinitionGroup.Key == (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlExternalOut)
                        count -= 1;
                }
                if (count > 0)
                {
                    string errorMessage = "This Account is undergoing Debt Counselling.";
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingReleaseandVariation",
      "If a Release & Variation is in progress, the further lending application must pend until the release & variation is complete.",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingReleaseandVariation")]
    [RuleInfo]
    public class ApplicationFurtherLendingReleaseandVariation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingReleaseandVariation rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IMortgageLoanAccount))
                throw new ArgumentException("The ApplicationFurtherLendingReleaseandVariation rule expects the following objects to be passed: IMortgageLoanAccount.");

            #endregion Check for allowed object type(s)

            IApplicationFurtherLending applicationFurtherLending = Parameters[0] as IApplicationFurtherLending;
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            bool releaseAndVariation = false;

            releaseAndVariation = stageDefinitionRepository.CheckCompositeStageDefinition(applicationFurtherLending.Key, stageDefinitionRepository.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ReadvancePayments, (int)StageDefinitions.ReleaseAndVariationRequest).Key);
            if (releaseAndVariation)
            {
                string errorMessage = "If a Release & Variation is in progress, the further lending application must pend until the release & variation is complete.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingLoanAgreementAmount",
  "",
    "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingLoanAgreementAmount")]
    [RuleInfo]
    public class ApplicationFurtherLendingLoanAgreementAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingLoanAgreementAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationFurtherLoan))
                throw new ArgumentException("The ApplicationFurtherLendingLoanAgreementAmount rule expects the following objects to be passed: IApplicationFurtherLoan.");

            #endregion Check for allowed object type(s)

            IApplicationFurtherLoan applicationFurtherLoan = Parameters[0] as IApplicationFurtherLoan;

            double furtherLoanAmount = 0.0;
            double loanCurrentBalance = 0.0;
            double bondLoanAgreementAmount = 0.0;

            // Get the furtherLoanAmount
            furtherLoanAmount = applicationFurtherLoan.RequestedCashAmount.Value;

            foreach (IFinancialService financialService in applicationFurtherLoan.Account.FinancialServices)
            {
                if (financialService == null)
                    return 0;

                // if finservType != VarLoan or FixedLoan and accStatus != open
                if (!((financialService.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan || financialService.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan)
                    && financialService.AccountStatus.Key == (int)AccountStatuses.Open))
                    return 0;

                // Get the loan current balance
                // Found the bond - get the bond value.
                IMortgageLoan mortgageLoan = financialService as IMortgageLoan;
                if (mortgageLoan == null)
                    return 0;

                loanCurrentBalance = mortgageLoan.CurrentBalance;

                foreach (IBond bond in mortgageLoan.Bonds)
                {
                    if (bond == null)
                        break;

                    //bond.LoanAgreements[0].Amount
                    foreach (ILoanAgreement la in bond.LoanAgreements)
                    {
                        if (la == null)
                            break;

                        // need to get the total sum of all loans in all bonds.
                        bondLoanAgreementAmount = bondLoanAgreementAmount + la.Amount;
                    }
                }
            }

            if ((loanCurrentBalance + furtherLoanAmount) >= bondLoanAgreementAmount)
            {
                string errorMessage = String.Format("The Loan Agreement Amount ({0:c}) is exceeded.", bondLoanAgreementAmount);
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }

            return 0;
        }
    }

    /// <summary>
    /// New Rule required to prevent Paying out of Further Lending on loans with HOC of "Paid Up with No HOC"
    /// </summary>
    [RuleDBTag("ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC",
    "Checks FL Application for HOC on main account has at least one of HOCInsurerKey = 23, or DetailTypeKey = 100, or HOCStatusKey = 3",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC", false)]
    [RuleInfo]
    public class ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("The ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan ml = Parameters[0] as IApplicationMortgageLoan;

            IAccount hocAccount = ml.RelatedAccounts.Where(x => x.Product.Key == (int)SAHL.Common.Globals.Products.HomeOwnersCover).SingleOrDefault();
            if (hocAccount != null)
            {
                IFinancialService hocFinancialService = hocAccount.FinancialServices.Where(y => y.FinancialServiceType.Key == (int)SAHL.Common.Globals.FinancialServiceTypes.HomeOwnersCover).SingleOrDefault();
                if (hocAccount != null && hocFinancialService != null)
                {
                    IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                    IHOC hoc = hocRepo.GetHOCByKey(hocFinancialService.Key);

                    if (hoc != null)
                    {
                        if (hoc.HOCInsurer.Key == (int)HOCInsurers.PaidupwithnoHOC)
                        {
                            string errorMessage = String.Format("The HOC insurer on the main loan account is set to 'Paid up with no HOC', please add HOC detail before continuing.");
                            AddMessage(errorMessage, errorMessage, Messages);
                            return 1;
                        }
                        if (hoc.HOCStatus.Key == (int)HocStatuses.PaidUpwithnoHOC)
                        {
                            string errorMessage = String.Format("The HOC status on the main loan account is set to 'Paid up with no HOC', please add HOC detail before continuing.");
                            AddMessage(errorMessage, errorMessage, Messages);
                            return 1;
                        }
                    }
                }
            }

            foreach (IDetail detail in ml.Account.Details)
            {
                if (detail.DetailType.Key == (int)DetailTypes.PaidUpWithNoHOC)
                {
                    string errorMessage = String.Format("There is a 'Paid up with no HOC' detail type against this loan, please amend before continuing.");
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 1;
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingNonperformingLoan",
    "A further lending application must not be allowed to be created if the loan is currently marked as a non-performing loan",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingNonperformingLoan")]
    [RuleInfo]
    public class ApplicationFurtherLendingNonperformingLoan : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingNonperformingLoan rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The ApplicationFurtherLendingNonperformingLoan rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IAccount acc = Parameters[0] as IAccount;
            IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

            if (financialServiceRepo.IsLoanNonPerforming(acc.Key))
            {
                string errorMessage = "A Further Loan application can not be processed on a loan that is currently marked as non-perfoming. Please refer this account to Litigation";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingRapidGoToCreditCheckLTV",
    "ApplicationFurtherLendingRapidGoToCreditCheckLTV.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingRapidGoToCreditCheckLTV")]
    [RuleInfo]
    public class ApplicationFurtherLendingRapidGoToCreditCheckLTV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingRapidGoToCreditCheckLTV rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;

            if (application == null)
                throw new ArgumentException("The ApplicationFurtherLendingRapidGoToCreditCheckLTV rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            // Return True if the application needs to go to credit or false if not
            if (!appRepo.RapidGoToCreditCheckLTV(application))
            {
                string msg = "The case needs to go to Credit.";
                AddMessage(msg, msg, Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ApplicationFurtherLendingAdditionalSuretyComposite",
    "ApplicationFurtherLendingAdditionalSuretyComposite",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.ApplicationFurtherLendingAdditionalSuretyComposite")]
    [RuleInfo]
    public class ApplicationFurtherLendingAdditionalSuretyComposite : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationFurtherLendingAdditionalSuretyComposite rule expects a Domain object to be passed.");

            IApplication app = Parameters[0] as IApplication;

            if (app == null)
                throw new ArgumentException("The ApplicationFurtherLendingAdditionalSuretyComposite rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IStageDefinitionRepository SDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            int countRA = SDRepo.CountCompositeStageOccurance(app.Key, (int)StageDefinitionStageDefinitionGroups.AdditionalSuretyOnRapid, false);
            int countFA = SDRepo.CountCompositeStageOccurance(app.Key, (int)StageDefinitionStageDefinitionGroups.AdditionalSuretyOnFA, false);

            if (countRA > 0 || countFA > 0)
            {
                string errorMessage = "This application has resulted in a surety being added to the loan account.";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("FurtherAdvanceBelowLAA",
 "Checks that a Further Advance application is below the Loan Agreement Amount. If so, return true",
   "SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.FurtherAdvanceBelowLAA")]
    [RuleInfo]
    public class FurtherAdvanceBelowLAA : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The FurtherAdvanceBelowLAA rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The FurtherAdvanceBelowLAA rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplication application = Parameters[0] as IApplication;
            if (application.ApplicationType.Key != (int)OfferTypes.FurtherAdvance)
                return 0;

            IApplicationFurtherAdvance applicationFurtherAdvance = Parameters[0] as IApplicationFurtherAdvance;

            //Get Loan Agreement Amount
            IAccount acc = applicationFurtherAdvance.Account;

            IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
            if (mla == null)
                throw new Exception("Not a Mortgage Loan Account!");

            double laTotal = 0;

            // Check all bonds against variable leg
            foreach (IBond bondV in mla.SecuredMortgageLoan.Bonds)
            {
                laTotal += bondV.BondLoanAgreementAmount;
            }

            // Check all bonds against fixed leg
            IAccountVariFixLoan facc = acc as IAccountVariFixLoan;
            if (facc != null)
            {
                foreach (IBond bondF in facc.FixedSecuredMortgageLoan.Bonds)
                {
                    laTotal += bondF.BondLoanAgreementAmount;
                }
            }

            //AccruedIinterest(V + F)
            double AccruedInterest = 0.00;
            foreach (IFinancialService financialService in mla.FinancialServices)
            {
                if (financialService.AccountStatus.Key == (int)AccountStatuses.Open)
                {
                    IMortgageLoan mortgageLoan = financialService as IMortgageLoan;
                    if (mortgageLoan != null)
                    {
                        AccruedInterest += mortgageLoan.AccruedInterestMTD.Value;
                    }
                }
            }

            //estimated disbursed amount = LoanBalance (V + F) + AccruedIinterest(V + F) + FAAmount

            //LoanBalance (V + F) = mla.LoanCurrentBalance
            //FAAmount = applicationFurtherAdvance.RequestedCashAmount

            double loanDisbursedValue = 0.00;
            loanDisbursedValue = mla.LoanCurrentBalance + AccruedInterest + applicationFurtherAdvance.RequestedCashAmount.Value; //mla.CapitalisedLife +

            //compare LAA above to loan value when this is disbursed
            if (Math.Round(loanDisbursedValue, 2) <= Math.Round(laTotal, 2))
            {
                // LAA is below the loan value so return true
                return 1;
            }

            string errorMessage = String.Format("Further Advance will result in a loan balance greater than the Loan Agreement Amount.");
            AddMessage(errorMessage, errorMessage, Messages);

            // LAA is above the loan value so return false
            return 0;
        }
    }

    [RuleDBTag("AccountForFurtherLendingApplicationIsAlphaHousing",
    "Determines whether the account for a further lending application is alpha housing.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing")]
    public class AccountForFurtherLendingApplicationIsAlphaHousing : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            var application = parameters[0] as IApplication;
            if (application is IApplicationFurtherLending)
            {
                var isAlphaHousing = application.Account.Details.Any(x => x.DetailType.Key == (int)DetailTypes.AlphaHousing);
                if (isAlphaHousing)
                {
                    string errorMessage = "Account is an Alpha Housing Loan";
                    AddMessage(errorMessage, errorMessage, messages);
                }
            }

            return 1;
        }
    }

    [RuleDBTag("AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate",
    "AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate")]
    [RuleInfo]
    public class AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            double existingRate = Convert.ToDouble(Parameters[0]);
            double newLinkRate = Convert.ToDouble(Parameters[1]);
            if (newLinkRate < existingRate)
            {
                AddMessage("The link rate may not be reduced during further lending for Alpha Housing loans", "The link rate may not be reduced during further lending for Alpha Housing loans", Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("LoanHas30YearTermAndRemainingInstalmentsCheck",
    "Caution: loan approved on 30 year term.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.FurtherLending.LoanHas30YearTermAndRemainingInstalmentsCheck")]
    public class LoanHas30YearTermAndRemainingInstalmentsCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new ArgumentException("The LoanHas30YearTermAndRemainingInstalmentsCheck rule expects 1 parameters.");
            }

            if (!(parameters[0] is IMortgageLoanAccount))
            {
                throw new ArgumentException("The LoanHas30YearTermAndRemainingInstalmentsCheck rule expects a Mortgage Loan Account.");
            }

            IMortgageLoanAccount mortgageLoanAccount = parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount != null && mortgageLoanAccount.IsThirtyYearTerm)
            {
                IMortgageLoan mortgageLoan = mortgageLoanAccount.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan).FirstOrDefault() as IMortgageLoan;
                if (mortgageLoan != null &&
                    mortgageLoan.RemainingInstallments > 240)
                {
                    AddMessage("Caution: loan approved on 30 year term.", "Caution: loan approved on 30 year term.", messages);
                    return 0;
                }
            }

            return 1;
        }
    }
}