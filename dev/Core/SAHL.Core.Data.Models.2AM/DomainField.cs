using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DomainFieldDataModel :  IDataModel
    {
        public DomainFieldDataModel(string domainFieldKey, string description, string displayDescription, int? formatTypeKey)
        {
            this.DomainFieldKey = domainFieldKey;
            this.Description = description;
            this.DisplayDescription = displayDescription;
            this.FormatTypeKey = formatTypeKey;
		
        }		

        public string DomainFieldKey { get; set; }

        public string Description { get; set; }

        public string DisplayDescription { get; set; }

        public int? FormatTypeKey { get; set; }
    }
}