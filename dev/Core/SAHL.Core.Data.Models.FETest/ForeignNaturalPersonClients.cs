using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class ForeignNaturalPersonClientsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ForeignNaturalPersonClientsDataModel(int legalEntityKey, int citizenshipTypeKey, string passportNumber)
        {
            this.LegalEntityKey = legalEntityKey;
            this.CitizenshipTypeKey = citizenshipTypeKey;
            this.PassportNumber = passportNumber;
		
        }
		[JsonConstructor]
        public ForeignNaturalPersonClientsDataModel(int id, int legalEntityKey, int citizenshipTypeKey, string passportNumber)
        {
            this.Id = id;
            this.LegalEntityKey = legalEntityKey;
            this.CitizenshipTypeKey = citizenshipTypeKey;
            this.PassportNumber = passportNumber;
		
        }		

        public int Id { get; set; }

        public int LegalEntityKey { get; set; }

        public int CitizenshipTypeKey { get; set; }

        public string PassportNumber { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}