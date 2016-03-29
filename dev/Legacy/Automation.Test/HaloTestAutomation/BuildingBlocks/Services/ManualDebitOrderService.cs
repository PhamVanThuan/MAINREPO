using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class ManualDebitOrderService : _2AMDataHelper, IManualDebitOrderService
    {
        private readonly IAccountService accountService;

        public ManualDebitOrderService()
        {
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
        }

        /// <summary>
        /// Inserts a manual debit order for an account and returns the insert manual debit order
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.ManualDebitOrder> InsertManualDebitOrder(Automation.DataModels.Account account, string userName)
        {
            var financialServiceKey = (from ml in account.FinancialServices
                                       where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                       select ml.FinancialServiceKey).FirstOrDefault();
            //get bank account
            var bankAccount = accountService.GetBankAccountRecordsForAccount(account.AccountKey).FirstOrDefault();
            base.InsertManualDebitOrder(financialServiceKey, bankAccount.BankAccountKey, userName);
            return GetManualDebitOrders(account.AccountKey);
        }
    }
}