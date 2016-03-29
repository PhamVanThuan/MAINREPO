using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AllocationMandateSetUserOrganisationStructureDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AllocationMandateSetUserOrganisationStructureDataModel(int allocationMandateSetKey, int userOrganisationStructureKey)
        {
            this.AllocationMandateSetKey = allocationMandateSetKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
		
        }
		[JsonConstructor]
        public AllocationMandateSetUserOrganisationStructureDataModel(int allocationMandateSetUserOrganisationStructureKey, int allocationMandateSetKey, int userOrganisationStructureKey)
        {
            this.AllocationMandateSetUserOrganisationStructureKey = allocationMandateSetUserOrganisationStructureKey;
            this.AllocationMandateSetKey = allocationMandateSetKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
		
        }		

        public int AllocationMandateSetUserOrganisationStructureKey { get; set; }

        public int AllocationMandateSetKey { get; set; }

        public int UserOrganisationStructureKey { get; set; }

        public void SetKey(int key)
        {
            this.AllocationMandateSetUserOrganisationStructureKey =  key;
        }
    }
}