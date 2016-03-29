using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.BankAccountDomain.Events
{
    public class BankAccountLinkedToClientEvent : Event
    {
        public int ClientKey { get; protected set; }

        public int ClientBankAccountKey { get; protected set; }

        public int BankAccountKey { get; protected set; }

        public string AccountName { get; protected set; }

        public string AccountNumber { get; protected set; }

        public string BranchCode { get; protected set; }

        public string BranchName { get; protected set; }

        public BankAccountLinkedToClientEvent(DateTime date, int bankAccountKey, int clientKey, int clientBankAccountKey, string accountName, string accountNumber, string branchCode, 
            string branchName)
            : base(date)
        {
            this.ClientKey = clientKey;
            this.BankAccountKey = bankAccountKey;
            this.ClientBankAccountKey = clientBankAccountKey;
            this.AccountName = accountName;
            this.AccountNumber = accountNumber;
            this.BranchCode = branchCode;
            this.BranchName = branchName;
        }
    }
}