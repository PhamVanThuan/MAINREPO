using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class VendorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public VendorDataModel(int? parentKey, string vendorCode, int organisationStructureKey, int legalEntityKey, int generalStatusKey)
        {
            this.ParentKey = parentKey;
            this.VendorCode = vendorCode;
            this.OrganisationStructureKey = organisationStructureKey;
            this.LegalEntityKey = legalEntityKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public VendorDataModel(int vendorKey, int? parentKey, string vendorCode, int organisationStructureKey, int legalEntityKey, int generalStatusKey)
        {
            this.VendorKey = vendorKey;
            this.ParentKey = parentKey;
            this.VendorCode = vendorCode;
            this.OrganisationStructureKey = organisationStructureKey;
            this.LegalEntityKey = legalEntityKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int VendorKey { get; set; }

        public int? ParentKey { get; set; }

        public string VendorCode { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.VendorKey =  key;
        }
    }
}