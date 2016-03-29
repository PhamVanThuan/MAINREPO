using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;

namespace SAHL.Common.BusinessModel.Rules.Affordability
{
    [RuleDBTag("DebtSettlementBankAccountUpdate",
  "Bank details can not be changed post disbursement",
  "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Affordability.DebtSettlementBankAccountUpdate")]
    [RuleInfo]
    public class DebtSettlementBankAccountUpdate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationDebtSettlement
                || Parameters[0] is IAccountDebtSettlement))
                throw new ArgumentException("The DebtSettlementBankAccountUpdate rule expects the following object(s) to be passed: IApplicationDebtSettlement or IAccountDebtSettlement.");

            IApplicationDebtSettlement appDebtSettlement = Parameters[0] as IApplicationDebtSettlement;

            IBankAccountRepository bRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IEventList<ILegalEntityBankAccount> leBankAccts;
            IReadOnlyEventList<IFinancialServiceBankAccount> fsb;

            IBankAccountSearchCriteria searchCriteria = new BankAccountSearchCriteria();

            if (Parameters[0] is IApplicationDebtSettlement)
            {
                if (appDebtSettlement.Key > 0) // only applies to update
                {
                    IApplicationDebtSettlement appExisting = FindExistingApplicationDebtSettlement(appDebtSettlement.Key);

                    if (appDebtSettlement.BankAccount.ACBBranch != null)
                    {
                        searchCriteria.ACBBranchKey = appDebtSettlement.BankAccount.ACBBranch.Key;
                    }
                    searchCriteria.ACBTypeKey = appDebtSettlement.BankAccount.ACBType.Key;
                    searchCriteria.AccountNumber = appDebtSettlement.BankAccount.AccountNumber;

                    leBankAccts = bRepo.SearchLegalEntityBankAccounts(searchCriteria, 5);
                    fsb = appDebtSettlement.BankAccount.GetFinancialServiceBankAccounts();

                    if (appExisting != null && appExisting.BankAccount != null && appExisting.BankAccount.Key != appDebtSettlement.BankAccount.Key && appDebtSettlement.OfferExpense.Application.Account.AccountStatus.Key == (int)AccountStatuses.Open)
                    {
                        AddMessage("Bank details can not be changed post disbursement.", "Bank details can not be changed post disbursement.", Messages);
                        return 0;
                    }
                    else
                        if ((leBankAccts != null && leBankAccts.Count > 1) || (fsb != null && fsb.Count > 1))
                    {
                        AddMessage("This bank account is linked to other entities and can not be updated", "This bank account is linked to other entities and can not be updated", Messages);
                        return 0;
                    }
                }
           }

           if (Parameters[0] is IAccountDebtSettlement)
           {
               IAccountDebtSettlement accDebtSettlement = Parameters[0] as IAccountDebtSettlement;

               if (accDebtSettlement.Key > 0) // only applies to update
               {
                   IAccountDebtSettlement accExisting = FindExistingAccountDebtSettlement(accDebtSettlement.Key);

                   if (accDebtSettlement.BankAccount.ACBBranch != null)
                   {
                       searchCriteria.ACBBranchKey = accDebtSettlement.BankAccount.ACBBranch.Key;
                   }
                   searchCriteria.ACBTypeKey = accDebtSettlement.BankAccount.ACBType.Key;
                   searchCriteria.AccountNumber = accDebtSettlement.BankAccount.AccountNumber;

                   leBankAccts = bRepo.SearchLegalEntityBankAccounts(searchCriteria, 5);
                   fsb = accDebtSettlement.BankAccount.GetFinancialServiceBankAccounts();

                   if (accExisting != null && accExisting.BankAccount != null && accExisting.BankAccount.Key != accDebtSettlement.BankAccount.Key && accDebtSettlement.AccountExpense.Account.AccountStatus.Key == (int)AccountStatuses.Open)
                   {
                       AddMessage("Bank details can not be changed post disbursement.", "Bank details can not be changed post disbursement.", Messages);
                       return 0;
                   }
                   else
                       if ((leBankAccts != null && leBankAccts.Count > 1) || (fsb != null && fsb.Count > 1))
                   {
                       AddMessage("This bank account is linked to other entities and can not be updated", "This bank account is linked to other entities and can not be updated", Messages);
                       return 0;
                   }
               }
           }

           return 0;
        }


        private IApplicationDebtSettlement FindExistingApplicationDebtSettlement(int key)
        {
            IApplicationDebtSettlement RetVal = null;

            string HQL = "Select a from ApplicationDebtSettlement_DAO a where a.Key = ?";

            SimpleQuery<ApplicationDebtSettlement_DAO> q = new SimpleQuery<ApplicationDebtSettlement_DAO>(HQL, key);
            ApplicationDebtSettlement_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
                 RetVal = new ApplicationDebtSettlement(res[0]);
            
            return RetVal;
        }

        private IAccountDebtSettlement FindExistingAccountDebtSettlement(int key)
        {
            IAccountDebtSettlement RetVal = null;

            string HQL = "Select a from AccountDebtSettlement_DAO a where a.Key = ?";

            SimpleQuery<AccountDebtSettlement_DAO> q = new SimpleQuery<AccountDebtSettlement_DAO>(HQL, key);
            AccountDebtSettlement_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
                RetVal = new AccountDebtSettlement(res[0]);

            return RetVal;
        }
    }
}
