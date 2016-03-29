using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.BusinessModel;
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

namespace SAHL.Common.BusinessModel.Rules.FixedDebitOrder
{
    [RuleDBTag("FinancialServiceDOEffectiveDateBusinessDay",
"Effective Date must be a business day and have one business day prior to effective date ",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceDOEffectiveDateBusinessDay")]
    [RuleInfo]
    public class FinancialServiceDOEffectiveDateBusinessDay : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange fdc = (IFutureDatedChange)Parameters[0];

            // if the value hasn't been set, exit
            if (fdc.EffectiveDate == DateTime.MinValue)
                return 1;

            ICalendar cal = Calendar.GetCalendarItemsByDate(fdc.EffectiveDate.Date);
            if (cal != null)
            {
                if (cal.IsHoliday || cal.IsSaturday || cal.IsSunday)
                    AddMessage("The Effective Date must be a business day ", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("FinancialServiceDOEffectiveDatePreviousDayBusinessDay",
   "Effective Date must be a business day and have one business day prior to effective date ",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceDOEffectiveDatePreviousDayBusinessDay")]
    [RuleInfo]
    public class FinancialServiceDOEffectiveDatePreviousDayBusinessDay : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange fdc = (IFutureDatedChange)Parameters[0];

            // if the value hasn't been set, exit
            if (fdc.EffectiveDate == DateTime.MinValue)
                return 1;

            ICalendar cal = Calendar.GetCalendarItemsByDate(fdc.EffectiveDate.AddDays(-1).Date);

            if (cal != null)
            {
                if (cal.IsHoliday || cal.IsSaturday || cal.IsSunday)
                    AddMessage("The Effective Date must have one business day prior to Effective Date ", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("FutureDatedChangeEffectiveDateMinimum",
       "Effective Date must be greater than today's date ",
       "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FutureDatedChangeEffectiveDateMinimum")]
    [RuleInfo]
    public class FutureDatedChangeEffectiveDateMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange fdc = (IFutureDatedChange)Parameters[0];

            if (fdc.EffectiveDate < DateTime.Today)
            {
                AddMessage("Effective Date must be greater than today's date", "", Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("FutureDatedChangeEffectiveDateMaximum",
    "Effective Date cannot be greater further than 6 months in the future",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FutureDatedChangeEffectiveDateMaximum")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MaximumMonths,6,9" })]
    public class FutureDatedChangeEffectiveDateMaximum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange fdc = (IFutureDatedChange)Parameters[0];
            int maxMonths = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (fdc.EffectiveDate > DateTime.Now.AddMonths(maxMonths))
            {
                string msg = String.Format("Effective Date cannot be more than {0} months from today's date", maxMonths);
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("FinancialServiceDebitOrderUpdateDayMonthDone",
   "There can not be more than 1 change of Debit Order details in the same month ",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceDebitOrderUpdateDayMonthDone")]
    [RuleInfo]
    public class FinancialServiceDebitOrderUpdateDayMonthDone : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange fdc = (IFutureDatedChange)Parameters[0];
            int numberOfChangesinSameMonth = 1;

            // Get all future dated change for that generic key
            IFutureDatedChangeRepository fdcRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
            IList<IFutureDatedChange> fdcList = fdcRepo.GetFutureDatedChangesByGenericKey(fdc.IdentifierReferenceKey, (int)SAHL.Common.Globals.FutureDatedChangeTypes.NormalDebitOrder);

            if (fdcList != null)
            {
                for (int i = 0; i < fdcList.Count; i++)
                {
                    if (fdcList[i].EffectiveDate.Month == fdc.EffectiveDate.Month)
                    {
                        for (int x = 0; x < fdcList[i].FutureDatedChangeDetails.Count; x++)
                        {
                            if (fdcList[i].FutureDatedChangeDetails[x].TableName == "FinancialServiceBankAccount")
                                numberOfChangesinSameMonth += 1;
                        }
                    }
                }
            }
            if (numberOfChangesinSameMonth > 1)
                AddMessage("There can not be more than 1 change of Debit Order details scheduled for same month ", "", Messages);

            return 1;
        }
    }

    [RuleDBTag("FinancialServiceBankAccountAddNewFixedDebitChangeReference",
    "Reference Key must be an Account Key",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountAddNewFixedDebitChangeReference")]
    [RuleInfo]
    public class FinancialServiceBankAccountAddNewFixedDebitChangeReference : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange fdc = (IFutureDatedChange)Parameters[0];
            IAccount acc;
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            if (fdc.FutureDatedChangeType.Key == (int)FutureDatedChangeTypes.FixedDebitOrder)
            {
                acc = accRepo.GetAccountByKey(fdc.IdentifierReferenceKey);

                if (acc == null)
                {
                    AddMessage("The Identifier Reference Key must be an Account Key", "", Messages);
                }
            }
            return 1;
        }
    }

    [RuleDBTag("FinancialServiceBankAccountAddNewDebitOrderChangeReference",
   "Reference Key must be an Financial Service Key",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountAddNewDebitOrderChangeReference")]
    [RuleInfo]
    public class FinancialServiceBankAccountAddNewDebitOrderChangeReference : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange fdc = (IFutureDatedChange)Parameters[0];
            IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            if (fdc.FutureDatedChangeType.Key == (int)FutureDatedChangeTypes.NormalDebitOrder)
            {
                IFinancialService fs = fsRepo.GetFinancialServiceByKey(Convert.ToInt32(fdc.IdentifierReferenceKey));

                if (fs == null)
                {
                    AddMessage("The Identifier Reference Key must be a Financial Service Key ", "", Messages);
                }
            }
            return 1;
        }
    }

    [RuleDBTag("FutureDatedChangeSinglePaymentCheck",
    "Checks that when creating a new fixed debit order, that one and only one debit order payment will occur per month",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FutureDatedChangeSinglePaymentCheck")]
    [RuleInfo]
    public class FutureDatedChangeSinglePaymentCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange futureDatedChange = Parameters[0] as IFutureDatedChange;
            if (futureDatedChange == null)
            {
                IFutureDatedChangeDetail futureDatedChangeDetail = Parameters[0] as IFutureDatedChangeDetail;
                futureDatedChange = futureDatedChangeDetail.FutureDatedChange;
            }

            if (futureDatedChange == null)
                throw new Exception("Rule FutureDatedChangeSinglePaymentCheck expected a parameter of type IFutureDatedChange or IFutureDatedChangeDetail");

            // this rule only applues to future dated changes for normal debit orders
            if (futureDatedChange.FutureDatedChangeType == null || futureDatedChange.FutureDatedChangeType.Key != (int)FutureDatedChangeTypes.NormalDebitOrder)
                return 1;

            IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IFutureDatedChangeRepository fdcRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

            // get the financial service
            IFinancialService financialService = fsRepo.GetFinancialServiceByKey(futureDatedChange.IdentifierReferenceKey);
            if (financialService.FinancialServiceBankAccounts.Count == 0)
                return 1;

            // get the CURRENT financial service bank account - if it's null we can exit
            IFinancialServiceBankAccount fsbaCurrent = financialService.CurrentBankAccount;
            if (fsbaCurrent == null)
                return 1;

            // get the financial service bank account related to the FutureDatedChange - if it's the same as current, exit
            IFinancialServiceBankAccount fsbaNew = null;
            foreach (IFutureDatedChangeDetail detail in futureDatedChange.FutureDatedChangeDetails)
            {
                if (String.Compare(detail.ColumnName, "DebitOrderDay", true) == 0)
                {
                    fsbaNew = fsRepo.GetFinancialServiceBankAccountByKey(detail.ReferenceKey);
                    break;
                }
            }
            if (fsbaNew.Key == fsbaCurrent.Key)
                return 1;

            // get a list of any future dated changes from the database - if there arent any then exit
            IList<IFutureDatedChange> lstFutureDatedChanges = fdcRepo.GetFutureDatedChangesByGenericKey(financialService.Key, (int)FutureDatedChangeTypes.NormalDebitOrder);

            // if were are 'adding' a new future dated change, check to see if it exists in the list already
            bool found = false;
            foreach (var fdc in lstFutureDatedChanges)
            {
                if (fdc.Key == futureDatedChange.Key)
                {
                    found = true;
                    break;
                }
            }

            // add it to the list above if it doesnt exit already
            if (found == false)
                lstFutureDatedChanges.Add(futureDatedChange);

            if (lstFutureDatedChanges.Count == 0)
                return 1;

            // loop thru them and do the following checks
            // 1. Compare the 1st Future Dated Change to the Current FSB
            // 2. Compare all subsequent Future Dated Changes to the previous Future Dated Change
            int idx = 0;
            int payments = 0;

            foreach (IFutureDatedChange fdc in lstFutureDatedChanges)
            {
                if (fdc.EffectiveDate.Date >= DateTime.Now.Date && fdc.FutureDatedChangeDetails != null)
                {
                    if (idx == 0) // this is our 1st Future Dated Change so lets check it against current FSB
                        payments = CheckPayments(fsbaCurrent.DebitOrderDay, GetDebitOrderDay(fdc), fdc.EffectiveDate);
                    else // subsequent Future Dated Changes so lets compare to the previous Future Dated Change
                        payments = CheckPayments(GetDebitOrderDay(lstFutureDatedChanges[idx - 1]), GetDebitOrderDay(fdc), fdc.EffectiveDate);

                    if (payments != 1)
                        break;

                    idx++;
                }
            }

            if (payments == 0)
            {
                AddMessage("A debit order with the selected Debit Order Day and Effective Date will result in a skipped payment", "", Messages);
                return 0;
            }
            else if (payments > 1)
            {
                AddMessage("A debit order with the selected Debit Order Day and Effective Date will result in more than one payment in a single month", "", Messages);
                return 0;
            }

            return 1;
        }

        private int GetDebitOrderDay(IFutureDatedChange fdc)
        {
            int debitOrderDay = 0;

            foreach (IFutureDatedChangeDetail detail in fdc.FutureDatedChangeDetails)
            {
                if (String.Compare(detail.ColumnName, "DebitOrderDay", true) == 0)
                {
                    debitOrderDay = Int32.Parse(detail.Value);
                    break;
                }
            }

            return debitOrderDay;
        }

        private int CheckPayments(int currentDebitOrderDay, int newDebitOrderDay, DateTime effectiveDate)
        {
            int day = 1;
            int payments = 0;

            while (day <= 28)
            {
                if (day == currentDebitOrderDay && day < effectiveDate.Day)
                    payments++;

                if (day == newDebitOrderDay && day >= effectiveDate.Day)
                    payments++;

                if (payments > 1)
                    break;

                day++;
            }

            return payments;
        }
    }

    [RuleDBTag("FutureDatedChangeEffectiveDateCheck",
    "The effective date of the future dated change cannot be same as the debit order day of the current debit order",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FutureDatedChangeEffectiveDateCheck")]
    [RuleInfo]
    public class FutureDatedChangeEffectiveDateCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange futureDatedChange = (IFutureDatedChange)Parameters[0];

            // this rule only applies to future dated changes for normal debit orders
            if (futureDatedChange.FutureDatedChangeType == null || futureDatedChange.FutureDatedChangeType.Key != (int)FutureDatedChangeTypes.NormalDebitOrder)
                return 1;

            IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

            // get the financial service and the CURRENT financial service bank account - if it's null we can exit
            IFinancialService financialService = fsRepo.GetFinancialServiceByKey(futureDatedChange.IdentifierReferenceKey);
            IFinancialServiceBankAccount fsbaCurrent = financialService.CurrentBankAccount;
            if (fsbaCurrent == null)
                return 1;

            string msg = "The Effective Date cannot be the same as the Debit Order Day of the current Debit Order";
            if (futureDatedChange.EffectiveDate.Day == fsbaCurrent.DebitOrderDay)
            {
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("DebitOrderFinancialServiceCheck",
    "Checks if there any applicable FinancialService/s to be updated.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.DebitOrderFinancialServiceCheck")]
    [RuleInfo]
    public class DebitOrderFinancialServiceCheck : BusinessRuleBase
    {
        public DebitOrderFinancialServiceCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAccount account = Parameters[0] as IAccount;

            if (account == null)
                throw new Exception("Rule DebitOrderFinancialServiceCheck expected a parameter of type IAccount");

            string sqlQuery = UIStatementRepository.GetStatement("Rules.FixedDebitOrder", "DebitOrderFinancialServiceCheck");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", account.Key));
            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(FinancialService_DAO), prms);

            if (o == null)
                return 1;

            string errorMessage = Convert.ToString(o);
            if (string.IsNullOrEmpty(errorMessage))
                return 1;

            AddMessage(errorMessage, errorMessage, Messages);
            return 1;
        }
    }

    [RuleDBTag("FutureDatedChangeEffectiveDateDODayCheck",
    "The effective dates Day of the future dated change cannot be same as the debit order day",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FutureDatedChangeEffectiveDateDODayCheck")]
    [RuleInfo]
    public class FutureDatedChangeEffectiveDateDODayCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFutureDatedChange futureDatedChange = Parameters[0] as IFutureDatedChange;
            if (futureDatedChange == null)
            {
                IFutureDatedChangeDetail futureDatedChangeDetail = Parameters[0] as IFutureDatedChangeDetail;
                futureDatedChange = futureDatedChangeDetail.FutureDatedChange;
            }

            if (futureDatedChange == null)
                throw new Exception("Rule FutureDatedChangeEffectiveDateDODayCheck expected a parameter of type IFutureDatedChange or IFutureDatedChangeDetail");

            // this rule only applues to future dated changes for normal debit orders
            if (futureDatedChange.FutureDatedChangeType == null || futureDatedChange.FutureDatedChangeType.Key != (int)FutureDatedChangeTypes.NormalDebitOrder)
                return 1;

            foreach (IFutureDatedChangeDetail fdcD in futureDatedChange.FutureDatedChangeDetails)
            {
                if (fdcD.ColumnName == "DebitOrderDay" && fdcD.TableName == "FinancialServiceBankAccount")
                {
                    int doDay = Convert.ToInt16(fdcD.Value);

                    if (doDay > 0 && futureDatedChange.EffectiveDate.Day == doDay)
                    {
                        string msg = "The Effective Date cannot be the same Day as the Debit Order Day, this will result in a skipped payment.";
                        AddMessage(msg, msg, Messages);
                        return 0;
                    }
                }
            }

            return 1;
        }
    }
}