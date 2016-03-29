using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OrganisationTypeDataModel :  IDataModel
    {
        public OrganisationTypeDataModel(int organisationTypeKey, string description)
        {
            this.OrganisationTypeKey = organisationTypeKey;
            this.Description = description;
		
        }		

        public int OrganisationTypeKey { get; set; }

        public string Description { get; set; }
    }
}