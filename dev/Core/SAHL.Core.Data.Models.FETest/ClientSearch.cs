using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class ClientSearchDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ClientSearchDataModel(string idNumber, string legalName, string email, bool hasMultipleRoles)
        {
            this.IdNumber = idNumber;
            this.LegalName = legalName;
            this.Email = email;
            this.HasMultipleRoles = hasMultipleRoles;
		
        }
		[JsonConstructor]
        public ClientSearchDataModel(int id, string idNumber, string legalName, string email, bool hasMultipleRoles)
        {
            this.Id = id;
            this.IdNumber = idNumber;
            this.LegalName = legalName;
            this.Email = email;
            this.HasMultipleRoles = hasMultipleRoles;
		
        }		

        public int Id { get; set; }

        public string IdNumber { get; set; }

        public string LegalName { get; set; }

        public string Email { get; set; }

        public bool HasMultipleRoles { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}