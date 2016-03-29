using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class Tmp_FailedLegalEntityPostalAddressDataModel :  IDataModel
    {
        public Tmp_FailedLegalEntityPostalAddressDataModel(int legalEntityKey, int? failedStreetMigrationKey, int? failedPostalMigrationKey, bool isCleaned)
        {
            this.LegalEntityKey = legalEntityKey;
            this.FailedStreetMigrationKey = failedStreetMigrationKey;
            this.FailedPostalMigrationKey = failedPostalMigrationKey;
            this.IsCleaned = isCleaned;
		
        }		

        public int LegalEntityKey { get; set; }

        public int? FailedStreetMigrationKey { get; set; }

        public int? FailedPostalMigrationKey { get; set; }

        public bool IsCleaned { get; set; }
    }
}