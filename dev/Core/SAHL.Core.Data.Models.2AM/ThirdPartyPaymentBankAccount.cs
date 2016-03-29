using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ThirdPartyPaymentBankAccountDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ThirdPartyPaymentBankAccountDataModel(int bankAccountKey, int thirdPartyKey, int generalStatusKey, string beneficiaryBankCode, string emailAddress)
        {
            this.BankAccountKey = bankAccountKey;
            this.ThirdPartyKey = thirdPartyKey;
            this.GeneralStatusKey = generalStatusKey;
            this.BeneficiaryBankCode = beneficiaryBankCode;
            this.EmailAddress = emailAddress;
		
        }
		[JsonConstructor]
        public ThirdPartyPaymentBankAccountDataModel(int thirdPartyPaymentBankAccountKey, Guid id, int bankAccountKey, int thirdPartyKey, int generalStatusKey, string beneficiaryBankCode, string emailAddress)
        {
            this.ThirdPartyPaymentBankAccountKey = thirdPartyPaymentBankAccountKey;
            this.Id = id;
            this.BankAccountKey = bankAccountKey;
            this.ThirdPartyKey = thirdPartyKey;
            this.GeneralStatusKey = generalStatusKey;
            this.BeneficiaryBankCode = beneficiaryBankCode;
            this.EmailAddress = emailAddress;
		
        }		

        public int ThirdPartyPaymentBankAccountKey { get; set; }

        public Guid Id { get; set; }

        public int BankAccountKey { get; set; }

        public int ThirdPartyKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public string BeneficiaryBankCode { get; set; }

        public string EmailAddress { get; set; }

        public void SetKey(int key)
        {
            this.ThirdPartyPaymentBankAccountKey =  key;
        }
    }
}