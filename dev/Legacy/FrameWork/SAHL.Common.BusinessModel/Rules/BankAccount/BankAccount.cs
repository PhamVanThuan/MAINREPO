using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.BankAccount
{
    public class BankAccountCDV
    {
    }

    [RuleDBTag("BankAccountCDVValidation",
    "This validates the bank account number.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.BankAccount.BankAccountCDVValidation")]
    [RuleInfo]
    public class BankAccountCDVValidation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length <= 0)
                throw new ArgumentException("This rules expects a parameter to be passed.");

            IBankAccount ba = Parameters[0] as IBankAccount;

            if (ba == null)
                throw new ArgumentException("Parameter[0] is not of type IBankAccount.");

            ICDVRepository cdvRepo = RepositoryFactory.GetRepository<ICDVRepository>();
            bool passed = false;

            if (ba.ACBBranch != null)
            {
                passed = cdvRepo.ValidateAccountNo(ba.ACBBranch.Key, ba.ACBType.Key, ba.AccountNumber);
            }

            if (!passed)
            {
                //string ErrorMessage = string.Format("Bank Account Number Validation Failed. 1) {0}. 2) Exeception Route - {1}.",cdvRepo.ErrorMessage,cdvRepo.ExceptionRoutine);
                if (ba.ACBBranch == null)
                {
                    AddMessage("Invalid ACBBranch", "Invalid ACBBranch", Messages);
                }
                else
                {
                    AddMessage(cdvRepo.ErrorMessage, cdvRepo.ErrorMessage, Messages);
                }
            }

            return 1;
        }
    }

    [RuleDBTag("BankAccountUnique",
    "This bank account already exists.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.BankAccount.BankAccountUnique")]
    [RuleInfo]
    public class BankAccountUnique : BusinessRuleBase
    {
        public BankAccountUnique(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IBankAccount))
                throw new ArgumentException("Parameter[0] is not of type IBankAccount.");

            IBankAccount ba = (IBankAccount)Parameters[0];

            if (ba.ACBBranch == null)
                return 0;

            string sqlQuery = UIStatementRepository.GetStatement("Rules.BankAccount", "BankAccountUnique");

            //ParameterCollection prms = new ParameterCollection();
            //Helper.AddVarcharParameter(prms, "@ACBBranchCode", ba.ACBBranch.Key.ToString().Trim());
            //Helper.AddVarcharParameter(prms, "@AccountNumber", ba.AccountNumber.ToString().Trim());
            //Helper.AddIntParameter(prms, "@BankAccountKey", ba.Key);

            //object o = Helper.ExecuteScalar(con, sqlQuery, prms);

            //if (o != null)
            //{
            //    string errorMessage = "This bank account already exists.";
            //    AddMessage(errorMessage, errorMessage, Messages);
            //    return 1;
            //}

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ACBBranchCode", ba.ACBBranch.Key.ToString().Trim()));
            parameters.Add(new SqlParameter("@AccountNumber", ba.AccountNumber.ToString().Trim()));
            parameters.Add(new SqlParameter("@BankAccountKey", ba.Key));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

            if (o != null)
            {
                string errorMessage = "This bank account already exists.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;

            /*
            if (!(Parameters[0] is IBankAccount))
                throw new ArgumentException("Parameter[0] is not of type IBankAccount.");

            IBankAccount ba = (IBankAccount)Parameters[0];
            IBankAccountRepository bRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();

            if (ba.ACBBranch == null)
            {
                return 0;
            }

            IBankAccount baDup = bRepo.GetBankAccountByACBBranchCodeAndAccountNumber(ba.ACBBranch.Key, ba.AccountNumber);

            if (baDup != null) // account exists
                if (ba.Key != baDup.Key) // and is not the account we are trying to save/update
                    AddMessage("This bank account already exists.", "This bank account already exists.", Messages);

            //if (ba.Key == 0 && bRepo.GetBankAccountByACBBranchCodeAndAccountNumber(ba.ACBBranch.Key, ba.AccountNumber) != null)
            //    AddMessage("This bank account already exists.", "This bank account already exists.", Messages);

            return 0;
             */
        }
    }

    //    [RuleDBTag("BankAccountUsedByDebitOrder",
    //"Cannot update a bank account if it is being used by a debit order.",
    //"SAHL.Rules.DLL",
    //"SAHL.Common.BusinessModel.Rules.BankAccount.BankAccountUsedByDebitOrder")]
    //    [RuleInfo]
    //    public class BankAccountUsedByDebitOrder : BusinessRuleBase
    //    {
    //        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //        {
    //            if (!(Parameters[0] is IBankAccount))
    //                throw new ArgumentException("Parameter[0] is not of type IBankAccount.");

    //            IBankAccount ba = (IBankAccount)Parameters[0];

    //            IBankAccountSearchCriteria searchCriteria = new BankAccountSearchCriteria();

    //            IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

    //            if (FSR.CountOccurencesOfBankAccountInFinancialServices(ba.Key) > 0)
    //            {
    //                AddMessage("This bank account is linked to a debit order and cannot be updated", "This bank account is linked to a debit order and cannot be updated", Messages);
    //            }
    //            return 0;
    //        }
    //    }

    [RuleDBTag("BankAccountUpdateNotUsed",
   "This bank account is linked to other entities and can not be updated.",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.BankAccount.BankAccountUpdateNotUsed")]
    [RuleInfo]
    public class BankAccountUpdateNotUsed : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IBankAccount))
                throw new ArgumentException("Parameter[0] is not of type IBankAccount.");

            IBankAccount ba = (IBankAccount)Parameters[0];
            IBankAccountRepository bRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            IBankAccountSearchCriteria searchCriteria = new BankAccountSearchCriteria();

            if (ba.ACBBranch != null)
            {
                searchCriteria.ACBBranchKey = ba.ACBBranch.Key;
                searchCriteria.ACBTypeKey = ba.ACBType.Key;
                searchCriteria.AccountNumber = ba.AccountNumber;
            }
            IEventList<ILegalEntityBankAccount> leBankAccts = bRepo.SearchLegalEntityBankAccounts(searchCriteria, 5);
            IReadOnlyEventList<IFinancialServiceBankAccount> fsb = ba.GetFinancialServiceBankAccounts();
            IApplicationExpense appExpense = appRepo.GetApplicationExpenseByBankAccountNameAndBankAccountNumber(ba.AccountName, ba.AccountNumber);

            if ((fsb != null && fsb.Count > 1) || (leBankAccts != null && leBankAccts.Count > 1) || appExpense != null)
                AddMessage("This bank account is linked to other entities and can not be updated", "This bank account is linked to other entities and can not be updated", Messages);

            return 0;
        }
    }
}