using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class BankAccountModelManager : IBankAccountModelManager
    {
        private IValidationUtils validationUtils;

        public BankAccountModelManager(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public List<BankAccountModel> PopulateBankAccounts(List<BankAccount> comcorpApplicantBankAccounts)
        {
            var bankAccounts = new List<BankAccountModel>();
            foreach (var comcorpBankAccount in comcorpApplicantBankAccounts)
            {
                ACBType accountType = validationUtils.ParseEnum<ACBType>(comcorpBankAccount.AccountType);
                BankAccountModel bankAccount = new BankAccountModel(
                    comcorpBankAccount.STDAccountBranchCode,
                    comcorpBankAccount.AccountBranch,
                    comcorpBankAccount.AccountNumber,
                    accountType,
                    comcorpBankAccount.AccountName,
                    null,
                    comcorpBankAccount.isMainAccount
                    );
                bankAccounts.Add(bankAccount);
            }
            return bankAccounts;
        }
    }
}