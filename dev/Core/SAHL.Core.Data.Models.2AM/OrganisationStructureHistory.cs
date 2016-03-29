using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OrganisationStructureHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OrganisationStructureHistoryDataModel(int organisationStructureKey, int? parentKey, string description, int organisationTypeKey, int generalStatusKey, DateTime changeDate, string action)
        {
            this.OrganisationStructureKey = organisationStructureKey;
            this.ParentKey = parentKey;
            this.Description = description;
            this.OrganisationTypeKey = organisationTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ChangeDate = changeDate;
            this.Action = action;
		
        }
		[JsonConstructor]
        public OrganisationStructureHistoryDataModel(int organisationStructureHistoryKey, int organisationStructureKey, int? parentKey, string description, int organisationTypeKey, int generalStatusKey, DateTime changeDate, string action)
        {
            this.OrganisationStructureHistoryKey = organisationStructureHistoryKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.ParentKey = parentKey;
            this.Description = description;
            this.OrganisationTypeKey = organisationTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ChangeDate = changeDate;
            this.Action = action;
		
        }		

        public int OrganisationStructureHistoryKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int? ParentKey { get; set; }

        public string Description { get; set; }

        public int OrganisationTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public string Action { get; set; }

        public void SetKey(int key)
        {
            this.OrganisationStructureHistoryKey =  key;
        }
    }
}