using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class NaturalPersonClientDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public NaturalPersonClientDataModel(int legalEntityKey, bool isActive, string idNumber)
        {
            this.LegalEntityKey = legalEntityKey;
            this.IsActive = isActive;
            this.IdNumber = idNumber;
		
        }
		[JsonConstructor]
        public NaturalPersonClientDataModel(int id, int legalEntityKey, bool isActive, string idNumber)
        {
            this.Id = id;
            this.LegalEntityKey = legalEntityKey;
            this.IsActive = isActive;
            this.IdNumber = idNumber;
		
        }		

        public int Id { get; set; }

        public int LegalEntityKey { get; set; }

        public bool IsActive { get; set; }

        public string IdNumber { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}