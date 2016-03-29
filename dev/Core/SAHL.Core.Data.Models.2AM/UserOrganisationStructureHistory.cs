using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserOrganisationStructureHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserOrganisationStructureHistoryDataModel(int userOrganisationStructureKey, int aDUserKey, int organisationStructureKey, DateTime? changeDate, string action, int? genericKey, int? genericKeyTypeKey, int generalStatusKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.ADUserKey = aDUserKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.ChangeDate = changeDate;
            this.Action = action;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public UserOrganisationStructureHistoryDataModel(int userOrganisationStructureHistoryKey, int userOrganisationStructureKey, int aDUserKey, int organisationStructureKey, DateTime? changeDate, string action, int? genericKey, int? genericKeyTypeKey, int generalStatusKey)
        {
            this.UserOrganisationStructureHistoryKey = userOrganisationStructureHistoryKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.ADUserKey = aDUserKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.ChangeDate = changeDate;
            this.Action = action;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int UserOrganisationStructureHistoryKey { get; set; }

        public int UserOrganisationStructureKey { get; set; }

        public int ADUserKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string Action { get; set; }

        public int? GenericKey { get; set; }

        public int? GenericKeyTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.UserOrganisationStructureHistoryKey =  key;
        }
    }
}