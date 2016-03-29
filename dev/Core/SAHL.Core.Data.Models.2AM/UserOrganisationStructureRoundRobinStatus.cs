using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserOrganisationStructureRoundRobinStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserOrganisationStructureRoundRobinStatusDataModel(int userOrganisationStructureKey, int generalStatusKey, int capitecGeneralStatusKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.GeneralStatusKey = generalStatusKey;
            this.CapitecGeneralStatusKey = capitecGeneralStatusKey;
		
        }
		[JsonConstructor]
        public UserOrganisationStructureRoundRobinStatusDataModel(int userOrganisationStructureRoundRobinStatusKey, int userOrganisationStructureKey, int generalStatusKey, int capitecGeneralStatusKey)
        {
            this.UserOrganisationStructureRoundRobinStatusKey = userOrganisationStructureRoundRobinStatusKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.GeneralStatusKey = generalStatusKey;
            this.CapitecGeneralStatusKey = capitecGeneralStatusKey;
		
        }		

        public int UserOrganisationStructureRoundRobinStatusKey { get; set; }

        public int UserOrganisationStructureKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public int CapitecGeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.UserOrganisationStructureRoundRobinStatusKey =  key;
        }
    }
}