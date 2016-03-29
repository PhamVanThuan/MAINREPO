using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class ClientAddressesDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ClientAddressesDataModel(int legalEntityAddressKey, int addressKey, int addressTypeKey, int addressFormatKey, int? legalEntityKey)
        {
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.AddressKey = addressKey;
            this.AddressTypeKey = addressTypeKey;
            this.AddressFormatKey = addressFormatKey;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public ClientAddressesDataModel(int id, int legalEntityAddressKey, int addressKey, int addressTypeKey, int addressFormatKey, int? legalEntityKey)
        {
            this.Id = id;
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.AddressKey = addressKey;
            this.AddressTypeKey = addressTypeKey;
            this.AddressFormatKey = addressFormatKey;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int Id { get; set; }

        public int LegalEntityAddressKey { get; set; }

        public int AddressKey { get; set; }

        public int AddressTypeKey { get; set; }

        public int AddressFormatKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}