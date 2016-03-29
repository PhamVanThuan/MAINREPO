using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PersalOrganisationCodesDataModel :  IDataModel
    {
        public PersalOrganisationCodesDataModel(string persalOrganisationKey, string persalOrganisationName)
        {
            this.PersalOrganisationKey = persalOrganisationKey;
            this.PersalOrganisationName = persalOrganisationName;
		
        }		

        public string PersalOrganisationKey { get; set; }

        public string PersalOrganisationName { get; set; }
    }
}