using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class BankAccount
    {
        //BankAccount
        public int BankAccountKey { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string UserID { get; set; }

        public DateTime ChangeDate { get; set; }

        //ACBBranch
        public string ACBBranchCode { get; set; }

        public string ACBBranchDescription { get; set; }

        public string ActiveIndicator { get; set; }

        public Branch Branch { get; set; }

        //ACBBank
        public int ACBBankCode { get; set; }

        public string ACBBankDescription { get; set; }

        //ACBType
        public ACBTypeEnum ACBTypeNumber { get; set; }

        public string ACBTypeDescription { get; set; }
    }
}