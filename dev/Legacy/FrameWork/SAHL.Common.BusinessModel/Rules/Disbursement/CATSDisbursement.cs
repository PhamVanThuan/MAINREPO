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

namespace SAHL.Common.BusinessModel.Rules.Disbursement
{
    [RuleDBTag("CATSDisbursementValidateUpdateRecord",
    "Disbursement may not be updated",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementValidateUpdateRecord")]
    [RuleInfo]
    public class CATSDisbursementValidateUpdateRecord : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                if (disbursement.DisbursementStatus.Key == (int)DisbursementStatuses.Disbursed)
                {
                    AddMessage("Disbursement may not be updated", "", Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("CATSDisbursementValidateAmount",
    "Disbursement must be greater than zero and less than R 10 000 000",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementValidateAmount")]
    [RuleInfo]
    public class CATSDisbursementValidateAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                if (disbursement.Amount <= 0 || disbursement.Amount > 10000000)
                {
                    AddMessage("Disbursement must be greater than zero and less than R 10 000 000", "", Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("CATSDisbursementValidateTypeCancRefundCurrentBalance",
    "Cancellation Refund may not be posted for this account",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementValidateTypeCancRefundCurrentBalance")]
    [RuleInfo]
    public class CATSDisbursementValidateTypeCancRefundCurrentBalance : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                IMortgageLoanAccount mlAcct = disbursement.Account as IMortgageLoanAccount;
                if (disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.CancellationRefund)
                {
                    //if (disbursement.DisbursementStatus.Key == (int)DisbursementStatuses.ReadyForDisbursement)
                    //{
                    //    if (mlAcct.LoanCurrentBalance > 0.01)
                    //    {
                    //        AddMessage("Cancellation Refund may not be posted for this account, the current balance is greater than R 0.00.", "", Messages);
                    //        return 0;
                    //    }
                    //}
                    //else if (mlAcct.LoanCurrentBalance > 0 || (Convert.ToDouble(mlAcct.LoanCurrentBalance) + Convert.ToDouble(disbursement.Amount) > 0.01 || (Convert.ToDouble(mlAcct.LoanCurrentBalance) + Convert.ToDouble(disbursement.Amount) < -0.01)))
                    //{
                    //    AddMessage("Cancellation Refund may not be saved for this account, resulting balance will not be R0.00 or outstanding Current Balance exists.", "", Messages);
                    //    return 0;
                    //}

                    double result = mlAcct.LoanCurrentBalance;
                    result += disbursement.Amount.HasValue ? disbursement.Amount.Value : 0;

                    if (result > 0.01)
                    {
                        AddMessage("Cancellation Refund may not be saved for this account, resulting balance will be > R 0.00", "", Messages);
                        return 0;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("CATSDisbursementValidateReadvanceDebtCounselling",
    "Client is under debt Counselling - Readvance may not be posted!",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementValidateReadvanceDebtCounselling")]
    [RuleInfo]
    public class CATSDisbursementValidateReadvanceDebtCounselling : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                if (disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.ReAdvance || disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.CAP2ReAdvance)
                {
                    IMortgageLoanAccount mlAcct = disbursement.Account as IMortgageLoanAccount;

                    if (mlAcct.UnderDebtCounselling)
                    {
                        AddMessage("Client is under Debt Counselling - Readvance may not be posted", "", Messages);
                        return 0;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("CATSDisbursementValidateTypeCAP2AddRecord",
    "No associated CAP2 Application found for this account!",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementValidateTypeCAP2AddRecord")]
    [RuleInfo]
    public class CATSDisbursementValidateTypeCAP2AddRecord : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                if (disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.CAP2ReAdvance)
                {
                    bool readvanceFound = false;

                    ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
                    IList<ICapApplication> cappApp = capRepo.GetCapOfferByAccountKey(disbursement.Account.Key);

                    if (cappApp != null && cappApp.Count > 0)
                    {
                        for (int i = 0; i < cappApp.Count; i++)
                        {
                            if (cappApp[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired)
                            {
                                readvanceFound = true;
                                break;
                            }
                        }
                    }

                    if (!readvanceFound)
                    {
                        AddMessage("No associated CAP2 Application found for this account!", "", Messages);
                        return 0;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("CATSDisbursementQuickCashDisbursementValidate",
    "An undisbursed regular payment must exist",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementQuickCashDisbursementValidate")]
    [RuleInfo]
    public class CATSDisbursementQuickCashDisbursementValidate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                if (disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.QuickCash)
                {
                    IQuickCashRepository qcRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
                    List<IApplicationInformationQuickCashDetail> qcDetails = null;

                    if (disbursement.Key > 0)
                        qcDetails = qcRepo.GetApplicationInformationQuickCashDetailFromDisbursementObj(disbursement);
                    else
                        qcDetails = qcRepo.GetApplicationInformationQuickCashDetailByAccountKey(disbursement.Account.Key);

                    bool validDisbursement = false;

                    for (int i = 0; i < qcDetails.Count; i++)
                    {
                        if (!Convert.ToBoolean(qcDetails[i].Disbursed) && Convert.ToDouble(qcDetails[i].RequestedAmount) > 0)
                            validDisbursement = true;
                    }
                    if (!validDisbursement)
                    {
                        AddMessage("An undisbursed Quick Cash payment must exist before a Quick Cash payment can be disbursed", "", Messages);
                        return 0;
                    }
                }
            }
            return 1;
        }
    }

    #region CATSDisbursementTransactionAmountValidate

    [RuleDBTag("CATSDisbursementTransactionAmountValidate",
    "The sum of the disbursement amounts must equal the sum of the transaction amounts",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementTransactionAmountValidate")]
    [RuleInfo]
    public class CATSDisbursementTransactionAmountValidate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            double disbursementAmount = (double)Parameters[0];
            double transactionAmount = (double)Parameters[1];

            if (disbursementAmount != transactionAmount)
            {
                string err = "The total of the CATS Disbursement transactions does not equal the total amount to disburse.";
                AddMessage(err, err, Messages);
                return 0;
            }
            return 1;
        }
    }

    #endregion CATSDisbursementTransactionAmountValidate

    #region CATSDisbursementLoanAgreementAmountValidate

    [RuleDBTag("CATSDisbursementLoanAgreementAmountValidate",
    "The disbursement amount for ReAdvance or CAP2ReAdvance can not exceed the Account Loan Agreement Amount.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementLoanAgreementAmountValidate")]
    [RuleInfo]
    public class CATSDisbursementLoanAgreementAmountValidate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];
            IMortgageLoanAccount ml = (IMortgageLoanAccount)Parameters[1];

            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            IControl control = controlRepo.GetControlByDescription("Readvance to Loan Agreement Tolerance");

            double tolerance = 1;
            if (control != null)
                tolerance = control.ControlNumeric.HasValue ? ((100 + control.ControlNumeric.Value) / 100) : 1;

            double LAA = ml.SecuredMortgageLoan.SumBondLoanAgreementAmounts() * tolerance;
            double newBalance = ml.LoanCurrentBalance;

            foreach (IDisbursement disbursement in disburseList)
            {
                //if DisbursementStatus == Pending, transaction does not exist, so use disbursement amounts as it is not in the LoanBalance
                if (disbursement.DisbursementStatus.Key == (int)DisbursementStatuses.Pending
                    && (disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.ReAdvance ||
                        disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.CAP2ReAdvance))
                {
                    newBalance += disbursement.Amount.HasValue ? disbursement.Amount.Value : 0D;
                }
            }

            if (newBalance > LAA)
            {
                string strErr = "The total amounts entered for this disbursement will exceed the Loan Agreement Amount by " + (newBalance - LAA).ToString(SAHL.Common.Constants.CurrencyFormat);
                AddMessage(strErr, strErr, Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion CATSDisbursementLoanAgreementAmountValidate

    #region CATSDisbursementReAdvanceNotDisbursed

    [RuleDBTag("CATSDisbursementReAdvanceNotDisbursed",
    "Checks that the Total Readvance Amount is equal to the application readvance amount and that the readvance value has not already been disbursed.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementReAdvanceNotDisbursed")]
    [RuleInfo]
    public class CATSDisbursementReAdvanceNotDisbursed : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            //NB!:  rules CATSDisbursementReAdvanceNotDisbursed and CATSDisbursementReAdvanceNotDisbursedExt must always be run in together!

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The CATSDisbursementReAdvanceNotDisbursed rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The CATSDisbursementReAdvanceNotDisbursed rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IAccount acc = Parameters[0] as IAccount;

            //acc comes from the disbursement which could have been cached, reload to prevent lazy load exceptions
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            acc = accRepo.GetAccountByKey(acc.Key);

            //RCS do not have an offer to run this rule against.
            if (acc.OriginationSource.Key == (int)OriginationSources.RCS)
                return 0;

            IApplication application = null;
            foreach (IApplication app in acc.Applications)
            {
                if (app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                {
                    int appTypeKey = app.ApplicationType.Key;

                    if (appTypeKey == (int)OfferTypes.ReAdvance || appTypeKey == (int)OfferTypes.FurtherAdvance)
                    {
                        if (application == null || appTypeKey < application.ApplicationType.Key)
                            application = app;
                    }
                }
            }

            if (application == null)
            {
                string msg = "No open application exists for this Disbursement.";
                AddMessage(msg, msg, Messages);
                return 1;
            }

            // if AppStatus != Open or AppInfoType != Accepted error message
            if (application.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
            {
                string appType = String.Empty;
                switch (application.ApplicationType.Key)
                {
                    case 2:
                        appType = "Readvance";
                        break;
                    case 3:
                        appType = "Further Advance";
                        break;
                    default:
                        break;
                }

                string msg = String.Format("Application {0} : {1} is open and not yet granted by Credit, you can only disburse an Open offer that has been granted.", appType, application.Key);
                AddMessage(msg, msg, Messages);
                return 1;
            }

            double totalReadvanceAmount = Convert.ToDouble(Parameters[1]);

            //need to check an open offer with loan transaction in the last 2 days does not exist
            //Get the variable fs
            IFinancialService vfs = null;
            foreach (IFinancialService fs in acc.FinancialServices)
            {
                if (fs.AccountStatus.Key == (int)AccountStatuses.Open && fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
                {
                    vfs = fs;
                    break;
                }
            }

            //This check has been moved into a seperate rule below, CATSDisbursementReAdvanceNotDisbursedExt
            //NB!:  rules CATSDisbursementReAdvanceNotDisbursed and CATSDisbursementReAdvanceNotDisbursedExt must always be run in together!

            //if (vfs != null)
            //{
            //    ILoanTransactionRepository ltrepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
            //    IFinancialTransaction lt = ltrepo.GetLastLoanTransactionByTransactionTypeAndFinancialServiceKey(TransactionTypes.Readvance, vfs.Key, false);
            //    if (lt != null && lt.Amount == totalReadvanceAmount && lt.InsertDate > DateTime.Now.AddDays(-2))
            //    {
            //        string msg = "An advance transaction for " + totalReadvanceAmount.ToString(SAHL.Common.Constants.CurrencyFormat) + " exists in the past 2 days. This has already been disbursed.";
            //        AddMessage(msg, msg, Messages);
            //        return 1;
            //    }
            //}

            ISupportsVariableLoanApplicationInformation svlai = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (svlai.VariableLoanInformation.LoanAgreementAmount != totalReadvanceAmount)
            {
                // throw an error - cannot disburse funds
                string msg = "Cannot disburse if Total advance Amount is not equal to the Application amount.";
                AddMessage(msg, msg, Messages);
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("CATSDisbursementReAdvanceNotDisbursedExt",
    "Checks that the Total Readvance Amount is equal to the application readvance amount and that the readvance value has not already been disbursed.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementReAdvanceNotDisbursedExt")]
    [RuleInfo]
    public class CATSDisbursementReAdvanceNotDisbursedExt : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            //NB!:  rules CATSDisbursementReAdvanceNotDisbursed and CATSDisbursementReAdvanceNotDisbursedExt must always be run in together!

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The CATSDisbursementReAdvanceNotDisbursed rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The CATSDisbursementReAdvanceNotDisbursed rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IAccount acc = Parameters[0] as IAccount;

            //acc comes from the disbursement which could have been cached, reload to prevent lazy load exceptions
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            acc = accRepo.GetAccountByKey(acc.Key);

            //RCS do not have an offer to run this rule against.
            if (acc.OriginationSource.Key == (int)OriginationSources.RCS)
                return 0;

            double totalReadvanceAmount = Convert.ToDouble(Parameters[1]);

            //need to check an open offer with loan transaction in the last 2 days does not exist
            //Get the variable fs
            IFinancialService vfs = null;
            foreach (IFinancialService fs in acc.FinancialServices)
            {
                if (fs.AccountStatus.Key == (int)AccountStatuses.Open && fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
                {
                    vfs = fs;
                    break;
                }
            }

            if (vfs != null)
            {
                ILoanTransactionRepository ltrepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                IFinancialTransaction lt = ltrepo.GetLastLoanTransactionByTransactionTypeAndFinancialServiceKey(TransactionTypes.Readvance, vfs.Key, false);
                if (lt != null && lt.Amount == totalReadvanceAmount && lt.InsertDate > DateTime.Now.AddDays(-2))
                {
                    string msg = "An advance transaction for " + totalReadvanceAmount.ToString(SAHL.Common.Constants.CurrencyFormat) + " exists in the past 2 days. This has already been disbursed.";
                    AddMessage(msg, msg, Messages);
                    return 1;
                }
            }

            return 0;
        }
    }

    #endregion CATSDisbursementReAdvanceNotDisbursed

    #region CATSDisbursementSPVCheck

    [RuleDBTag("CATSDisbursementSPVCheck",
    "This checks if a SPV has been discontinued. If it has it throws an error as cannot disburse.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementSPVCheck")]
    [RuleInfo]
    public class CATSDisbursementSPVCheck : BusinessRuleBase
    {
        public CATSDisbursementSPVCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                //disbursement.LoanTransactions[0].SPVNumber
                string sqlQuery = UIStatementRepository.GetStatement("Rule", "DisbursementSPVCheck");
                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@accKey", disbursement.Account.Key));
                prms.Add(new SqlParameter("@disTransTypeKey", disbursement.DisbursementTransactionType.Key));

                //when adding 0 sql prms values the 0 can not be hardcoded
                int zero = 0;
                prms.Add(new SqlParameter("@OfferTypeKey", zero));

                object obj = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

                if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
                {
                    string errMsg = (string)obj;
                    AddMessage(errMsg, errMsg, Messages);
                }
            }
            return 0;
        }
    }

    #endregion CATSDisbursementSPVCheck

    #region CATSDisbursementDebitOrderSuspendedCapReAdvance

    [RuleDBTag("CATSDisbursementDebitOrderSuspendedCapReAdvance",
    "This stops the further advance and rapid from being disbursed from CATS disbursement screens if the debit order is suspended",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementDebitOrderSuspendedCapReAdvance", false)]
    [RuleInfo]
    public class CATSDisbursementDebitOrderSuspendedCapReAdvance : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            foreach (IDisbursement disbursement in disburseList)
            {
                if (disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.ReAdvance
                    || disbursement.DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.CAP2ReAdvance)
                {
                    IAccount acc = accRepo.GetAccountByKey(disbursement.Account.Key);
                    if (acc.Details.Count > 0)
                    {
                        foreach (IDetail detail in acc.Details)
                        {
                            if (detail.DetailType.Key == (int)DetailTypes.DebitOrderSuspended)
                            {
                                string msg = "The Debit Order is currently suspended against this application.";
                                AddMessage(msg, msg, Messages);
                                return 1;
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }

    #endregion CATSDisbursementDebitOrderSuspendedCapReAdvance

    #region Rollback Disbursement

    [RuleDBTag("CATSDisbursementRollback",
    "Checks that the Disbursement has not been disbursed and the the time is before 12:30 pm.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.CATSDisbursementRollback")]
    [RuleParameterTag(new string[] { "@CutOffTime,5,12:30" })]
    [RuleInfo]
    public class CATSDisbursementRollback : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            TimeSpan ts = TimeSpan.Parse(RuleItem.RuleParameters[0].Value);

            //if after CutOffTime, return false
            if (DateTime.Now.TimeOfDay > ts)
            {
                string msg = String.Format(@"Rollback of Disbursements is not allowed after: {0}", ts.ToString());
                AddMessage(msg, msg, Messages);
                return 0;
            }

            IReadOnlyEventList<IDisbursement> disburseList = (IReadOnlyEventList<IDisbursement>)Parameters[0];

            foreach (IDisbursement disbursement in disburseList)
            {
                if (disbursement.DisbursementStatus.Key == (int)DisbursementStatuses.Disbursed)
                {
                    string msg = String.Format(@"This item has already been disbursed and can never be rolled back.");
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    #endregion Rollback Disbursement

    #region DisbursementCutOffTimeCheck

    [RuleDBTag("DisbursementCutOffTimeCheck",
    "Check the disbursement is being posted before the cut off time.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disbursement.DisbursementCutOffTimeCheck")]
    [RuleParameterTag(new string[] { "@MaxTime,5,12:30" })]
    [RuleInfo]
    public class DisbursementCutOffTimeCheck : BusinessRuleBase
    {
        ICommonRepository commonRepository;

        public DisbursementCutOffTimeCheck(ICommonRepository commonRepository)
        {
            this.commonRepository = commonRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            TimeSpan ts = TimeSpan.Parse(RuleItem.RuleParameters[0].Value);

            //if after CutOffTime, return false
            if (commonRepository.GetTodaysDate().TimeOfDay > ts)
            {
                string msg = String.Format(@"Disbursements are not allowed after: {0}", ts.ToString());
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion DisbursementCutOffTimeCheck
}