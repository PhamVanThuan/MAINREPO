using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class LegalEntityBankAccount
    {
        public int LegalEntityKey { get; set; }

        public Automation.DataModels.LegalEntity LegalEntity { get; set; } // Model

        public int BankAccountKey { get; set; }

        public Automation.DataModels.BankAccount BankAccount { get; set; } // Model

        public int LegalEntityBankAccountKey { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public string UserID { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}