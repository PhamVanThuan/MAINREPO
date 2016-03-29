using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class ThirdPartiesDataModel :  IDataModel
    {
        public ThirdPartiesDataModel(int thirdPartyKey, int legalEntityKey, string tradingName, string contact, int generalStatusKey, int? genericKey, int? genericKeyTypeKey, string genericKeyTypeDescription, bool hasBankAccount, string paymentEmailAddress, int? bankAccountKey, string bankName, string branchCode, string branchName, string accountName, string accountNumber, string accountType)
        {
            this.ThirdPartyKey = thirdPartyKey;
            this.LegalEntityKey = legalEntityKey;
            this.TradingName = tradingName;
            this.Contact = contact;
            this.GeneralStatusKey = generalStatusKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKeyTypeDescription = genericKeyTypeDescription;
            this.HasBankAccount = hasBankAccount;
            this.PaymentEmailAddress = paymentEmailAddress;
            this.BankAccountKey = bankAccountKey;
            this.BankName = bankName;
            this.BranchCode = branchCode;
            this.BranchName = branchName;
            this.AccountName = accountName;
            this.AccountNumber = accountNumber;
            this.AccountType = accountType;
		
        }
		[JsonConstructor]
        public ThirdPartiesDataModel(int thirdPartyKey, Guid id, int legalEntityKey, string tradingName, string contact, int generalStatusKey, int? genericKey, int? genericKeyTypeKey, string genericKeyTypeDescription, bool hasBankAccount, string paymentEmailAddress, int? bankAccountKey, string bankName, string branchCode, string branchName, string accountName, string accountNumber, string accountType)
        {
            this.ThirdPartyKey = thirdPartyKey;
            this.Id = id;
            this.LegalEntityKey = legalEntityKey;
            this.TradingName = tradingName;
            this.Contact = contact;
            this.GeneralStatusKey = generalStatusKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKeyTypeDescription = genericKeyTypeDescription;
            this.HasBankAccount = hasBankAccount;
            this.PaymentEmailAddress = paymentEmailAddress;
            this.BankAccountKey = bankAccountKey;
            this.BankName = bankName;
            this.BranchCode = branchCode;
            this.BranchName = branchName;
            this.AccountName = accountName;
            this.AccountNumber = accountNumber;
            this.AccountType = accountType;
		
        }		

        public int ThirdPartyKey { get; set; }

        public Guid Id { get; set; }

        public int LegalEntityKey { get; set; }

        public string TradingName { get; set; }

        public string Contact { get; set; }

        public int GeneralStatusKey { get; set; }

        public int? GenericKey { get; set; }

        public int? GenericKeyTypeKey { get; set; }

        public string GenericKeyTypeDescription { get; set; }

        public bool HasBankAccount { get; set; }

        public string PaymentEmailAddress { get; set; }

        public int? BankAccountKey { get; set; }

        public string BankName { get; set; }

        public string BranchCode { get; set; }

        public string BranchName { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string AccountType { get; set; }
    }
}