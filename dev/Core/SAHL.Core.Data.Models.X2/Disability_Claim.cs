using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Disability_ClaimDataModel :  IDataModel
    {
        public Disability_ClaimDataModel(long instanceID, int disabilityClaimKey, string migrationTargetState, int genericKey)
        {
            this.InstanceID = instanceID;
            this.DisabilityClaimKey = disabilityClaimKey;
            this.MigrationTargetState = migrationTargetState;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int DisabilityClaimKey { get; set; }

        public string MigrationTargetState { get; set; }

        public int GenericKey { get; set; }
    }
}