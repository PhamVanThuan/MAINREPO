using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DomainProcessStatusDataModel :  IDataModel
    {
        public DomainProcessStatusDataModel(int domainProcessStatusKey, string description)
        {
            this.DomainProcessStatusKey = domainProcessStatusKey;
            this.Description = description;
		
        }		

        public int DomainProcessStatusKey { get; set; }

        public string Description { get; set; }
    }
}