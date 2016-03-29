using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DebtCounsellorDetailDataModel :  IDataModel
    {
        public DebtCounsellorDetailDataModel(int legalEntityKey, string nCRDCRegistrationNumber)
        {
            this.LegalEntityKey = legalEntityKey;
            this.NCRDCRegistrationNumber = nCRDCRegistrationNumber;
		
        }		

        public int LegalEntityKey { get; set; }

        public string NCRDCRegistrationNumber { get; set; }
    }
}