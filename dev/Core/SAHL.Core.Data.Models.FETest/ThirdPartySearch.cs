using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class ThirdPartySearchDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ThirdPartySearchDataModel(string legalName, string email)
        {
            this.LegalName = legalName;
            this.Email = email;
		
        }
		[JsonConstructor]
        public ThirdPartySearchDataModel(int id, string legalName, string email)
        {
            this.Id = id;
            this.LegalName = legalName;
            this.Email = email;
		
        }		

        public int Id { get; set; }

        public string LegalName { get; set; }

        public string Email { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}