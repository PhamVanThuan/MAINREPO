using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class HaloUsersDataModel :  IDataModel
    {
        public HaloUsersDataModel(int aDUserKey, string aDUserName, string legalEntityKey, int userOrganisationStructureKey, string orgStructureDescription, string capabilities)
        {
            this.ADUserKey = aDUserKey;
            this.ADUserName = aDUserName;
            this.LegalEntityKey = legalEntityKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.OrgStructureDescription = orgStructureDescription;
            this.Capabilities = capabilities;
		
        }		

        public int ADUserKey { get; set; }

        public string ADUserName { get; set; }

        public string LegalEntityKey { get; set; }

        public int UserOrganisationStructureKey { get; set; }

        public string OrgStructureDescription { get; set; }

        public string Capabilities { get; set; }
    }
}