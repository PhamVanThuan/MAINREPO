using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AllocationMandateSetGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AllocationMandateSetGroupDataModel(string allocationGroupName, int organisationStructureKey)
        {
            this.AllocationGroupName = allocationGroupName;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }
		[JsonConstructor]
        public AllocationMandateSetGroupDataModel(int allocationMandateSetGroupKey, string allocationGroupName, int organisationStructureKey)
        {
            this.AllocationMandateSetGroupKey = allocationMandateSetGroupKey;
            this.AllocationGroupName = allocationGroupName;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }		

        public int AllocationMandateSetGroupKey { get; set; }

        public string AllocationGroupName { get; set; }

        public int OrganisationStructureKey { get; set; }

        public void SetKey(int key)
        {
            this.AllocationMandateSetGroupKey =  key;
        }
    }
}